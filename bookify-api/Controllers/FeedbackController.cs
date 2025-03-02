using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFeedbackDTO>> GetFeedbackById(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null)
                return NotFound(new { message = "Feedback not found" });

            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFeedback([FromBody] AddFeedbackDTO addFeedbackDTO)
        {
            bool isCreated = await _feedbackService.CreateFeedbackAsync(addFeedbackDTO);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create feedback" });

            return StatusCode(201, new { message = "Feedback created successfully" });
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateFeedbackDTO updateFeedbackDto)
        {
            bool isUpdated = await _feedbackService.UpdateFeedbackAsync(id, updateFeedbackDto);
            if (!isUpdated)
                return NotFound(new { message = "Feedback not found or update failed" });

            return NoContent(); // HTTP 204
        }
    }
}
