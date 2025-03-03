using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static bookify_data.Helper.CacheKey;

namespace bookify_data.Repository
{
	public class AuthenRepository : IAuthenRepository
	{
		private readonly IConfiguration _configuration;
		private readonly BookifyDbContext _dbcontext;
		private readonly IRoleRepository _roleRepository;
        /*private readonly IEmailSender emailSender;*/

        public AuthenRepository(IConfiguration configuration, BookifyDbContext dbcontext, IRoleRepository roleRepository
            /*, IEmailSender emailSender*/
            )
		{
			_configuration = configuration;
			_dbcontext = dbcontext;
            _roleRepository = roleRepository;
            /*	this.emailSender = emailSender;*/
        }

		public async Task<string> Login(LoginModel model)
		{
			var user = await _dbcontext.Accounts.Include(u => u.Role).AsNoTracking()
											 /*.Where(x => x.Status.ToLower() == "active")*/
											 .SingleOrDefaultAsync(u => u.Email.Equals(model.Email));

			if (user == null)
			{
				return null;
			}

			if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
			{
				return null;
			}
			var claims = new[]
				{
				new Claim("AccountId", user.AccountId.ToString()),
			new Claim(ClaimTypes.Name, user.DisplayName),
			new Claim(ClaimTypes.NameIdentifier, user.DisplayName.ToString()),
			new Claim(ClaimTypes.Role, user.Role.RoleName),
			new Claim("RoleId", user.RoleId.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			/*new Claim("Avatar", user.Avatar.ToString()),*/
			   };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				_configuration["Jwt:Issuer"],
				_configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}




		public async Task<string> Register(RegisterLoginModel registerDTO)
		{
			try
			{
				var existingEmail = await _dbcontext.Accounts.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);
				if (existingEmail != null)
				{
					return "Email already exists";
				}

				var existingUser = await _dbcontext.Accounts.FirstOrDefaultAsync(u => u.Username == registerDTO.UserName);
				if (existingUser != null)
				{
					return "UserName already exists";
				}
				var existingPhone = await _dbcontext.Accounts.FirstOrDefaultAsync(u => u.Phone == registerDTO.PhoneNumber);
				if (existingPhone != null)
				{
					return "Phone number already exists";
				}
				var roleUser = await _roleRepository.GetByNameAsync("User");
				if (roleUser == null)
				{
                    return "Role User not found";
                }
				int id = roleUser.RoleId;

                var newUser = new Account
				{
					Username = registerDTO.UserName,
					DisplayName = registerDTO.FullName,
					Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
					Phone = registerDTO.PhoneNumber,
					Email = registerDTO.Email,
					/*ReferralCode = GenerateReferralCode(),*/
					/*DOB = registerDTO.DOB,*/
                    RoleId = id,
					/*Status = registerDTO.Status,*/
				};
				//if (registerDTO.Certification != null)
				//{
				//    var certificationUrl = await UploadFileAsync(registerDTO.Certification, "certification");
				//    newUser.Certification = certificationUrl;
				//}
				/*if (registerDTO.Avatar != null)
				{
					var avatarUrl = await UploadFileAsync(registerDTO.Avatar, "avatar");
					newUser.Avatar = avatarUrl;
				}*/

			/*	if (registerDTO.RoleId == 1)
				{
					Bookmark bookmark = new Bookmark
					{
						UserName = newUser.UserName,
					};
					newUser.Bookmark = bookmark;
				}
				else if (registerDTO.RoleId == 2)
				{
					var newInfo = new InstructorInfo
					{
						Username = registerDTO.UserName,
						Status = "Active"
					};
					newUser.InstructorInfos = newInfo;
				}*/


			/*	Wallet wallet = new Wallet
				{
					UserName = newUser.UserName,
					Balance = 0,
					TransactionTime = DateTime.UtcNow.AddHours(7),
				};
				newUser.Wallet = wallet;*/




				_dbcontext.Accounts.Add(newUser);
				await _dbcontext.SaveChangesAsync();

				return "User registered successfully.";
			}
			catch (Exception ex)
			{
				return $"Internal server error: {ex.Message}";
			}
		}



		/*public async Task<string> UploadFileAsync(IFormFile file)
		{
			if (file.Length > 0)
			{
				var stream = file.OpenReadStream();
				var bucket = _configuration["FireBase:Bucket"];

				var task = new FirebaseStorage(bucket)
					.Child("certifications")
					.Child(file.FileName)
					.PutAsync(stream);

				var downloadUrl = await task;

				return downloadUrl;
			}
			return null;
		}*/



		public string GenerateJwtToken(Account user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user), "User cannot be null.");
			}

			var claims = new[]
			{
			new Claim(ClaimTypes.Name, user.DisplayName ?? string.Empty),
			new Claim(ClaimTypes.NameIdentifier, user.DisplayName?.ToString() ?? string.Empty),
			new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Admin" : user.RoleId == 2 ? "Instructor" : "Student"),
			new Claim("RoleId", user.RoleId.ToString() ?? string.Empty),
			new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
			};



			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				_configuration["Jwt:Issuer"],
				_configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		/*public async Task<string> UploadFileAsync(IFormFile file, string fileType)
		{
			if (file.Length > 0)
			{
				var stream = file.OpenReadStream();
				var bucket = _configuration["FireBase:Bucket"];

				string folderName = fileType == "certification" ? "certifications" : "avatars";

				var task = new FirebaseStorage(bucket)
					.Child(folderName)
					.Child(file.FileName)
					.PutAsync(stream);

				var downloadUrl = await task;

				return downloadUrl;
			}
			return null;
		}*/

		public async Task<bool> Logout(string userId)
		{
			return await Task.FromResult(true);
		}
		private string GenerateReferralCode(int length = 8)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}


	}
}
