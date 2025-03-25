using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/feedbacks")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetFeedbackDTO>>> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllAsync();
            return Ok(feedbacks);
        }
        [HttpGet("{bookId}")]
        public async Task<ActionResult<GetFeedbackDTO>> GetFeedbackByBookId(int bookId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByBookIdAsync(bookId);

            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFeedbackDTO>> GetFeedbackById(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null)
                return NotFound(new { message = "Feedback not found" });

            return Ok(feedback);
        }



        [HttpPost]
        public async Task<IActionResult> CreateFeedbackIfOrdered([FromBody] AddFeedbackDTO addFeedbackDto)
        {
            try
            {
                bool isCreated = await _feedbackService.CreateFeedbackAsync(addFeedbackDto);
                if (!isCreated)
                    return BadRequest(new { message = "Failed to create feedback" });
                return StatusCode(201, new { message = "Feedback created successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{feedbackId}")]
        public async Task<IActionResult> ConfirmFeedBack(int feedbackId, [FromBody] ConfirmFeedbackDTO confirmFeedbackDto)
        {
            try
            {
                bool isConfirmed = await _feedbackService.ConfirmFeedBack(feedbackId, confirmFeedbackDto.Star, confirmFeedbackDto.FeedbackContent);
                if (!isConfirmed)
                    return NotFound(new { message = "Feedback not found or update failed" });
                return Ok("Feedback confirmed successfully");
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateFeedbackDTO updateFeedbackDto)
        {
            bool isUpdated = await _feedbackService.UpdateFeedbackAsync(id, updateFeedbackDto);
            if (!isUpdated)
                return NotFound(new { message = "Feedback not found or update failed" });

            return NoContent(); // HTTP 204
        }

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> UpdateFeedbackStatus(int id, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _feedbackService.UpdateFeedbackStatusAsync(id, status);

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
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            try
            {
                bool isDeleted = await _feedbackService.DeleteFeedbackAsync(id);
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
