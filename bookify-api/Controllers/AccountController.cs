using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using bookify_data.Entities;
using bookify_service.Interfaces;
using bookify_data.Model;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;

	public AccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	// PUT: api/Account/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountModel model)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var account = await _accountService.GetAccountByIdAsync(id);
		if (account == null)
		{
			return NotFound("Account không tồn tại");
		}

		// Ánh xạ các trường cần cập nhật từ DTO vào entity
		if (!string.IsNullOrEmpty(model.Password))
			account.Password = model.Password;

		if (!string.IsNullOrEmpty(model.Email))
			account.Email = model.Email;

		if (!string.IsNullOrEmpty(model.Phone))
			account.Phone = model.Phone;


		await _accountService.UpdateAccountAsync(account);

		return NoContent(); 
	}
}
