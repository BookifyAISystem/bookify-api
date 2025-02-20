using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using bookify_data.Entities;
using bookify_data.Helper;
using bookify_data.Model;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using bookify_data.Interfaces;
using bookify_data.Data;

namespace bookify_data.Repository
{
	public interface IAuthenticationRepository : IEntityRepository<Account>
	{
		AuthenticateResponse Authenticate(Account model, string ipAddress, string referer = null);
		AuthenticateResponse RefreshToken(string token, string ipAddress, string referer = null);
		Account GetUserByKey(string key);
		Account GetUserByID(decimal ID);
		Account GetCacheByCusID(string cusId);
        void UpdateClientUseApp(Account user);
		void ClearCache(string id);
		void ClearCache(decimal id);
		Account GetUserCurrent();
		//User UserCurrent();
		//bool CreateUserAdminCompany(Company company);
		Task<Account> CheckForgotPass(string url);
		/*bool CheckPassword(Account user, AuthenticateRequest request);
		bool ChangePassword(Account user, string password);*/

		/*Task<Account> Register(RegisterRequest model);*/
		Task UpdateCustomerInStore(decimal userId, string phone);
		Task UpdateAvatar(decimal userId, string avatar, string avatarThumb);
		string CreatePassword(string password, string passwordSalt);
	}
	public class AuthenticationRepository : EntityRepository<Account>, IAuthenticationRepository
	{
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ICacheService _cacheService;

		public AuthenticationRepository(IMapper mapper, IHttpContextAccessor contextAccessor, ICacheService cacheService)
		{
			_mapper = mapper;
			_contextAccessor = contextAccessor;
			_cacheService = cacheService;
		}

		#region Token Methods

		/// <summary>
		/// Xác thực người dùng và tạo token cùng refresh token.
		/// </summary>
		public AuthenticateResponse Authenticate(Account user, string ipAddress, string referer)
		{
			var jwtToken = GenerateJwtToken(user, referer);

			var refreshToken = GenerateRefreshToken(ipAddress, user.AccountId);

			string claims = string.Empty;

			return new AuthenticateResponse(user, jwtToken, refreshToken, claims);
		}
		public void UpdateClientUseApp(Account user)
		{
			Updates(user);
		}
		/// <summary>
		/// Làm mới token dựa vào refresh token hiện tại.
		/// </summary>
		public AuthenticateResponse RefreshToken(string token, string ipAddress, string referer)
		{
			RefreshToken refreshTokenObj = DecryptRefreshToken(token);

			if (refreshTokenObj == null || !refreshTokenObj.IsActive || refreshTokenObj.IsExpired)
				return null;

			// Tạo refresh token mới
			var newRefreshToken = GenerateRefreshToken(ipAddress, refreshTokenObj.userId);

			// Lấy thông tin người dùng từ database (ở đây dùng phương thức GetUserByID)
			Account user = GetUserByID(refreshTokenObj.userId);
			if (user == null || string.IsNullOrWhiteSpace(user.Password))
				return null;

			// Tạo JWT token mới
			var jwtToken = GenerateJwtToken(user, referer);

			return new AuthenticateResponse(user, jwtToken, newRefreshToken, string.Empty);
		}

		/// <summary>
		/// Tạo JWT token từ thông tin của user.
		/// </summary>
		private string GenerateJwtToken(Account user, string referer)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(AppSettings.Secret);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
				new Claim("id", user.AccountId.ToString()),
				new Claim(ClaimTypes.Name, user.DisplayName)
			};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = AppSettings.Url,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(AppSettings.ExpiredTime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		/// <summary>
		/// Tạo refresh token mới.
		/// </summary>
		private string GenerateRefreshToken(string ipAddress, decimal userId)
		{
			RefreshToken result;
			using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				var randomBytes = new byte[64];
				rngCryptoServiceProvider.GetBytes(randomBytes);
				result = new RefreshToken
				{
					userId = userId,
					Token = Convert.ToBase64String(randomBytes),
					Expires = DateTime.UtcNow.AddDays(30),
					Created = DateTime.UtcNow,
					CreatedByIp = ipAddress
				};
			}

			// Mã hóa refresh token để gửi về client
			string token = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(result));
			return token;
		}

		/// <summary>
		/// Giải mã refresh token.
		/// </summary>
		private RefreshToken DecryptRefreshToken(string refreshToken)
		{
			try
			{
				return EncryptionHelper.Decrypt<RefreshToken>(refreshToken);
			}
			catch
			{
				return null;
			}
		}

		#endregion

		/// <summary>
		/// Lấy thông tin người dùng hiện tại dựa vào context của HTTP request.
		/// </summary>
		public Account GetUserCurrent()
		{
			try
			{
				// Tìm claim có tên "sub" hoặc "id"
				string userId = _contextAccessor.HttpContext.User?.FindFirst("sub")?.Value
								?? _contextAccessor.HttpContext.User?.FindFirst("id")?.Value;
				if (string.IsNullOrWhiteSpace(userId))
					return null;

				if (decimal.TryParse(userId, out decimal id))
				{
					return GetUserByID(id);
				}
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Kiểm tra và lấy thông tin người dùng khi yêu cầu quên mật khẩu.
		/// </summary>
		public async Task<Account> CheckForgotPass(string url)
		{
			try
			{
				// Giải mã url (thay thế %2f thành "/" và %3d thành "=")
				var regexSlash = new Regex("%2f", RegexOptions.IgnoreCase);
				var regexEqual = new Regex("%3d", RegexOptions.IgnoreCase);
				url = regexSlash.Replace(url, "/");
				url = regexEqual.Replace(url, "=");

				// Giải mã tham số từ url
				List<string> args = StringHelper.DecrypParamUrl(Consts.String.KEY_CRYTO, url);

				// Truy vấn user theo Email hoặc Phone và mã xác thực
				var query = @"SELECT TOP 1 * FROM MasterCustomer WITH(NOLOCK) 
                              WHERE (Email LIKE @sendTo OR Phone LIKE @sendTo)
                                AND VerifyiedCode = @code AND LEN(VerifyiedCode) = 6";
				Account user = await _connection.QueryFirstOrDefaultAsync<Account>(query, new
				{
					sendTo = args[0],
					code = args[1]
				});
				return user;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Kiểm tra mật khẩu của user so với mật khẩu nhập vào.
		/// </summary>
		/*public bool CheckPassword(Account user, AuthenticateRequest request)
		{
			var hashedPassword = CreatePassword(request.Password, user.PasswordSalt);
			return user.Password == hashedPassword;
		}*/
		private RefreshToken RefreshTokenDecrypt(string refreshToken)
		{
			using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				var randomBytes = new byte[64];
				rngCryptoServiceProvider.GetBytes(randomBytes);
				return EncryptionHelper.Decrypt<RefreshToken>(refreshToken);
			}
		}

		//public string CreateAuthenticationTicket(User user)
		//{
		//    HttpContext httpContext = _contextAccessor.HttpContext;
		//    var key = Encoding.ASCII.GetBytes(SiteKeys.Token);

		//    var JWToken = new JwtSecurityToken(
		//    issuer: SiteKeys.UrlAppMobile,
		//    audience: SiteKeys.UrlAppMobile,
		//    claims: GetUserClaims(user),
		//    // claims: new[] { new Claim("id", user.ID) },// GetUserClaims(user),
		//    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
		//    expires: new DateTimeOffset(DateTime.Now.AddDays(7)).DateTime,
		//    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		//    );

		//    var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
		//    httpContext.Session.SetString("JWToken", token);

		//    httpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
		//    return token;
		//}

		

		public void SetCacheUser(Account user)
		{
			var userKey = string.Format(CacheKey.User.UserLogin, user.AccountId);
			var dic = new Dictionary<string, Account> { { userKey, user } };
			new CacheService().Set(userKey, dic, CacheTimes.OneWeek);
		}

		public Account GetCacheUser(decimal id)
		{
			Account result;
			var userKey = string.Format(CacheKey.User.UserLogin, id);
			var userCacheKey = new CacheService().Get<Dictionary<string, Account>>(userKey);
			if (userCacheKey != null)
			{
				result = userCacheKey.First().Value;
			}
			else
			{
				result = new CacheContext().Get<Account>(id);
				SetCacheUser(result);
			}

			return result;
		}

		public Account GetCacheByCusID(string cusId)
		{
			Account result;
			var userKey = string.Format(CacheKey.User.CustomerID, cusId);
			var userCacheKey = new CacheService().Get<Dictionary<string, Account>>(userKey);
			if (userCacheKey != null)
			{
				result = userCacheKey.First().Value;
			}
			else
			{

				result = new CacheContext().Find<Account>("CustomerID=@ID", new { ID = cusId });
				SetCacheUser(result);
			}

			return result;
		}
		public void ClearCache(decimal id)
		{
			ClearCache(id.ToString());
		}
		public void SetCacheToken(string userID, string token)
		{
			var userKey = string.Format(CacheKey.User.UserToken, userID);
			var dic = new Dictionary<string, string> { { userKey, token } };
			new CacheService().Set(userKey, dic, CacheTimes.OneWeek);
		}

		public string GetCacheToken(string id)
		{
			string result = "";
			var userKey = string.Format(CacheKey.User.UserToken, id);
			var userCacheKey = new CacheService().Get<Dictionary<string, string>>(userKey);
			if (userCacheKey != null)
			{
				result = userCacheKey.First().Value;
			}

			return result;
		}
		public void SetCacheRefreshToken(RefreshToken refreshToken)
		{
			var userKey = string.Format(CacheKey.User.UserRefreshToken, refreshToken.Token);
			var dic = new Dictionary<string, RefreshToken> { { userKey, refreshToken } };
			new CacheService().Set(userKey, dic, CacheTimes.OneMonth);
		}

		public RefreshToken GetCacheRefreshToken(string refreshToken)
		{
			RefreshToken result = new RefreshToken();
			var userKey = string.Format(CacheKey.User.UserRefreshToken, refreshToken);
			var userCacheKey = new CacheService().Get<Dictionary<string, RefreshToken>>(userKey);
			if (userCacheKey != null)
			{
				result = userCacheKey.First().Value;
			}

			return result;
		}


		public void ClearCache(string id)
		{
			var Key = string.Format(CacheKey.User.UserLogin, id);
			_cacheService.ClearStartsWith(Key);
		}
		/// <summary>
		/// Mã hóa mật khẩu với passwordSalt.
		/// </summary>
		public string CreatePassword(string password, string passwordSalt)
		{
			// Nếu passwordSalt rỗng thì tạo mới (ví dụ dùng StringHelper.CreatePin)
			var ps = string.IsNullOrWhiteSpace(passwordSalt) ? StringHelper.CreatePin() : passwordSalt;
			string pass = StringHelper.Encrypt(password, ps);
			return pass;
		}

		/// <summary>
		/// Thay đổi mật khẩu cho người dùng.
		/// </summary>
		/*public bool ChangePassword(Account user, string password)
		{
			user.PasswordSalt = Guid.NewGuid().ToString();
			user.Password = CreatePassword(password, user.PasswordSalt);
			Updates(user); // Giả sử phương thức này cập nhật user vào database
			return true;
		}*/

		/// <summary>
		/// Đăng ký tài khoản mới.
		/// Sử dụng AutoMapper để chuyển đổi từ RegisterRequest sang Account.
		/// </summary>
		/*public async Task<Account> Register(RegisterRequest model)
		{
			// Ánh xạ RegisterRequest sang đối tượng Account
			Account user = _mapper.Map<Account>(model);

			// Sinh password salt và mã hóa mật khẩu
			user.PasswordSalt = Guid.NewGuid().ToString();
			user.Password = CreatePassword(model.Password, user.PasswordSalt);

			*//*user.AccountId = Inserts();*//*

			return await Task.FromResult(user);
		}*/

		/// <summary>
		/// Cập nhật thông tin khách hàng trong cửa hàng.
		/// </summary>
		public async Task UpdateCustomerInStore(decimal userId, string phone)
		{
			// Giả sử nếu số điện thoại dài hơn 6 ký tự, lấy phần sau 6 ký tự
			string lastPhone = phone.Length > 6 ? phone.Substring(6) : "";
			var sql = "UPDATE CutomerInStore SET LastPhoneNumber = @LastPhoneNumber WHERE CustomerID = @UserId";
			await _connection.ExecuteAsync(sql, new { LastPhoneNumber = lastPhone, UserId = userId });
		}

		/// <summary>
		/// Cập nhật avatar cho người dùng.
		/// </summary>
		public async Task UpdateAvatar(decimal userId, string avatar, string avatarThumb)
		{
			var sql = @"UPDATE MasterCustomer 
                        SET Avatar = @avatar,
                            AvatarThumb = @avatarThumb
                        WHERE CustomerID = @userId";
			await _connection.ExecuteAsync(sql, new { userId, avatar, avatarThumb });
		}

		/// <summary>
		/// Lấy thông tin người dùng theo key (Email hoặc ContactPhone).
		/// </summary>
		public Account GetUserByKey(string key)
		{
			var sql = @"SELECT TOP 1 * FROM MasterCustomer WITH(NOLOCK)
                        WHERE (ContactPhone = @Key OR Email = @Key)
                          AND ISNULL(IsDelete, 0) = 0
                        ORDER BY CustomerID DESC";
			return _connection.Query<Account>(sql, new { Key = key }).FirstOrDefault();
		}

		/// <summary>
		/// Lấy thông tin người dùng theo CustomerID.
		/// </summary>
		public Account GetUserByID(decimal id)
		{
			var sql = "SELECT TOP 1 * FROM MasterCustomer WITH(NOLOCK) WHERE CustomerID = @ID";
			return _connection.Query<Account>(sql, new { ID = id }).FirstOrDefault();
		}
	}
}
