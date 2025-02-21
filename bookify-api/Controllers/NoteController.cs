using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService NoteService)
        {
            _noteService = NoteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteModel>>> GetAll()
        {
            var notes = await _noteService.GetAllAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteModel>> GetById(int id)
        {
            var note = await _noteService.GetByIdAsync(id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpGet("account-id/{accountId}")]
        public async Task<ActionResult<List<NoteModel>>> GetByAccountId(int accountId)
        {
            var notes = await _noteService.GetByAccountIdAsync(accountId);
            if (notes == null) return NotFound();
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteCreateRequest request)
        {

            try
            {
                var note = await _noteService.CreateAsync(request.Content, request.Status, request.AccountId);
                return CreatedAtAction(nameof(GetById), new { id = note.NoteId }, note);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NoteUpdateRequest request)
        {

            try
            {
                var updatedNote = await _noteService.UpdateAsync(id, request.Content, request.Status);
                if (updatedNote == null)
                {
                    return NotFound("Note not found.");
                }

                return Ok(updatedNote);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _noteService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

        [HttpPatch("change-status/{id}")]
        public async Task<ActionResult> ChangeStatus(int id, [FromBody] int status)
        {

            bool changed = await _noteService.ChangeStatus(id, status);
            if (!changed) return NotFound();
            return Ok();
        }

        public class NoteCreateRequest
        {
            public string Content { get; set; }
            public int Status { get; set; }
            public int AccountId { get; set; }
        }

        public class NoteUpdateRequest
        {
            public string Content { get; set; }
            public int Status { get; set; }
        }

    }
}
