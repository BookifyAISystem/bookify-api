﻿using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/vouchers")]
    [ApiController]
    public class VoucherController : Controller
    {

        private readonly IVoucherService _voucherService;

        public VoucherController (IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetVoucherDTO>>> GetAllVouchers()
        {
            var vouchers = await _voucherService.GetAllAsync();
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetVoucherDTO>> GetVoucherById(int id)
        {
            var voucher = await _voucherService.GetByIdAsync(id);
            if (voucher == null)
                return NotFound(new { message = "Voucher not found" });

            return Ok(voucher);
        }


        [HttpPost]
        public async Task<ActionResult> CreateVoucher([FromBody] AddVoucherDTO addVoucherDto)
        {
            bool isCreated = await _voucherService.CreateVoucherAsync(addVoucherDto);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create voucher" });

            return StatusCode(201, new { message = "Voucher created successfully" });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVoucher(int id, [FromBody] UpdateVoucherDTO updateVoucherDto)
        {
            bool isUpdated = await _voucherService.UpdateVoucherAsync(id, updateVoucherDto);
            if (!isUpdated)
                return NotFound(new { message = "Voucher not found or update failed" });

            return NoContent(); // HTTP 204
        }


        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> UpdateVoucherStatus(int id, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _voucherService.UpdateVoucherStatusAsync(id, status);

                if (!isUpdate)
                {
                    return NotFound($"Not found or update failed");
                }
                return Ok("Update Successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoucher(int id)
        {
            try
            {
                bool isDeleted = await _voucherService.DeleteVoucherAsync(id);
                if (!isDeleted)
                    return NotFound(new { message = "Not found or delete failed" });

                return Ok("Delete Success (Status = 0).");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
