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
	[HttpPut]
	[Route("updateAccount")]

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
	[HttpGet]
	[Route("deleteAccount")]

	public async Task<IActionResult> GetAccount(int id)
	{
		var account = await _accountService.GetAccountWithReferencesAsync(id);
		if (account == null)
			return NotFound();

		return Ok(account);
	}

	// DELETE: api/Account/5  (thực hiện soft delete)
	[HttpDelete]
	[Route("deleteAccount")]

	public async Task<IActionResult> DeleteAccount(int id)
	{
		bool result = await _accountService.DeleteAccountAsync(id);
		if (!result)
		{
			return NotFound("Account không tồn tại hoặc không thể xóa.");
		}

		return NoContent(); // 204
	}
	[HttpGet]
	[Route("getAccountsPaging")]

	public async Task<IActionResult> GetAccountsPaging([FromQuery] AccountQueryParameters parameters)
	{
		var pagedResult = await _accountService.GetAccountsAsync(parameters);
		return Ok(pagedResult);
	}
}
