using bookify_data.DTOs;
using bookify_data.DTOs.BookshelfDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api/book-shelf")]
    [ApiController]
    public class BookshelfController : ControllerBase
    {
        private readonly IBookshelfService _bookshelfService;

        public BookshelfController(IBookshelfService bookshelfService)
        {
            _bookshelfService = bookshelfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookshelves()
        {
            return Ok(await _bookshelfService.GetAllBookshelvesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookshelfById(int id)
        {
            var bookshelf = await _bookshelfService.GetBookshelfByIdAsync(id);
            return bookshelf != null ? Ok(bookshelf) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddBookshelf(CreateBookshelfDTO dto)
        {
            await _bookshelfService.AddBookshelfAsync(dto);
            return Ok(new { message = "Bookshelf added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookshelf(int id, UpdateBookshelfDTO dto)
        {
            await _bookshelfService.UpdateBookshelfAsync(dto);
            return Ok(new { message = "Bookshelf updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookshelf(int id)
        {
            await _bookshelfService.DeleteBookshelfAsync(id);
            return Ok(new { message = "Bookshelf deleted successfully!" });
        }
    }
}
