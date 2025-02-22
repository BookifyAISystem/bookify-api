using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewsModel>>> GetAll()
        {
            var news = await _newsService.GetAllAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsModel>> GetById(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news == null) return NotFound();
            return Ok(news);
        }

        [HttpGet("account-id/{accountId}")]
        public async Task<ActionResult<List<NewsModel>>> GetByAccountId(int accountId)
        {
            var news = await _newsService.GetByAccountIdAsync(accountId);
            if (news == null) return NotFound();
            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.Summary) || string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                return BadRequest("Title, content, summary, and image URL are required.");
            }
            try
            {
                var news = await _newsService.CreateAsync(request.Title, request.Content, request.Summary, request.ImageUrl, request.PublishAt, request.AccountId, request.Status);
                return CreatedAtAction(nameof(GetById), new { id = news.NewsId }, news);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewsUpdateRequest request)
        {
            try
            {
                var news = await _newsService.UpdateAsync(id, request.Title, request.Content, request.Summary, request.ImageUrl, request.PublishAt, request.Status);
                return CreatedAtAction(nameof(GetById), new { id = news.NewsId }, news);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsService.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"News with ID {id} not found.");
            }
            return Ok("Deleted successfully.");
        }

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] int status)
        {
            var result = await _newsService.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"News with ID {id} not found.");
            }
            return Ok("Changed successfully.");
        }


        public class NewsCreateRequest
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Summary { get; set; }
            public string ImageUrl { get; set; }
            public DateTime PublishAt { get; set; }
            public int AccountId { get; set; }
            public int Status { get; set; }
        }

        public class NewsUpdateRequest
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Summary { get; set; }
            public string ImageUrl { get; set; }
            public DateTime PublishAt { get; set; }
            public int Status { get; set; }
        }
    }
}
