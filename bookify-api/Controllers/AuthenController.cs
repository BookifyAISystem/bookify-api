using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bookify_api.Controllers
{
	[Route("api/authen")]
	[ApiController]
	[EnableCors("AllowAll")]

	public class AuthenController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IAuthenServices _authenServices;
		private readonly IUnitOfWork _unitOfWork;
		/*private readonly IUserServices _userServices;*/
		/*private readonly IEmailSender emailSender;*/
		public AuthenController(IConfiguration configuration, IAuthenServices authenServices, IUnitOfWork unitOfWork
			/*, IUserServices userServices*/
			/*, IEmailSender emailSender*/
			)
		{
			_configuration = configuration;
			_authenServices = authenServices;
			/*_userServices = userServices;*/

			_unitOfWork = unitOfWork;
			/*this.emailSender = emailSender;*/
		}
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
			{
				return BadRequest(new { code = 400, message = "Invalid username or password" });
			}

			try
			{
				_unitOfWork.BeginTransaction();

				var token = await _authenServices.Login(model);

				if (string.IsNullOrEmpty((string?)token))
				{
					return Unauthorized(new { code = 401, message = "Invalid username or password" });
				}

				// Lưu token vào cookie HTTP-only
				var cookieOptions = new CookieOptions
				{
					HttpOnly = false,
					Secure = true,   // Chạy trên HTTPS
					SameSite = SameSiteMode.None, // Hoặc SameSiteMode.Lax nếu chỉ cần GET requests
					Expires = DateTime.Now.AddDays(7),
					Path = "/",
					//Domain = "coursev1.vercel.app"
				};
				Response.Cookies.Append("authToken", (string)token, cookieOptions);
				_unitOfWork.CommitTransaction();
				return Ok(new { code = 200, token = token, message = "Login successful" });
			}
			catch (Exception ex)
			{
				_unitOfWork.RollbackTransaction();
				return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = ex.Message });
			}
		}


		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromForm] RegisterLoginModel registerDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				_unitOfWork.BeginTransaction();

				var result = await _authenServices.Register(registerDTO);

				if (result.StartsWith("Internal server error"))
				{
					_unitOfWork.RollbackTransaction();
					return StatusCode(500, result);
				}
				else if (result == "Email already exists" || result == "UserName already exists" || result == "Phone number already exists")
				{
					_unitOfWork.RollbackTransaction();
					return BadRequest(result);
				}

				_unitOfWork.CommitTransaction();
				return Ok(result);
			}
			catch (Exception ex)
			{
				_unitOfWork.RollbackTransaction();
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
			}
		}


		

		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest("Invalid token.");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = _configuration["Jwt:Issuer"],
					ValidAudience = _configuration["Jwt:Audience"],
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

				/*var result = await _authenServices.ConfirmEmailAsync(username);*/
				return Content(@"
            <html>
                <head>
                    <script type='text/javascript'>
                        alert('Your account has been successfully confirmed.');
                    </script>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            text-align: center;
                            padding-top: 50px;
                        }
                        .notification {
                            font-size: 24px;
                            color: green;
                        }
                    </style>
                </head>
                <body>
                    <div class='notification'>Your account has been successfully confirmed.</div>
                </body>
            </html>", "text/html");

			}
			catch (SecurityTokenExpiredException)
			{
				return BadRequest("Token has expired.");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
			}
		}




		/*[HttpPost("send-verification-email")]
		public async Task<IActionResult> SendVerificationEmail([FromQuery] string username)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				return BadRequest(new { message = "Username cannot be null or empty." });
			}

			try
			{
				var user = await _userServices.GetUserByUserName(username);

				if (user == null)
				{
					return NotFound("User not found.");
				}

				if (user.isVerify == true)
				{
					return Ok("Email have been verified");
				}

				string emailBody = emailSender.GetMailBody(new RegisterLoginModel
				{
					UserName = user.UserName,
					Email = user.Email
				});

				bool emailSent = await emailSender.EmailSendAsync(user.Email, "Verify your account", emailBody);

				if (!emailSent)
				{
					return StatusCode(500, "Failed to send verification email.");
				}

				return Ok("Pls check email to verify.");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
			}
		}*/

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var result = await _authenServices.Logout(HttpContext);
			if (result)
			{
				return Ok(new { message = "Logout successful" });
			}
			return BadRequest(new { message = "Logout failed" });
		}
	}
}
