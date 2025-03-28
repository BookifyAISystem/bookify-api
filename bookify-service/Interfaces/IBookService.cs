﻿using bookify_data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookService
    {
        Task<(IEnumerable<GetBookDTO> Books, int TotalCount)> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<GetBookDTO?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(AddBookDTO bookDto);
        Task UpdateBookAsync(UpdateBookDTO bookDto);
        Task DeleteBookAsync(int bookId);
        Task<IEnumerable<GetBookDTO>> SuggestBooksAsync(string query, int limit);
        Task<(IEnumerable<GetBookDTO>, int)> SearchBooksAsync(string query, int pageNumber, int pageSize);
        Task UpdateStatusAsync(int bookId, int status);
        Task<IEnumerable<GetBookDTO>> GetLatestBooksAsync(int count);
        Task<IEnumerable<GetBookDTO>> GetBestSellingBooksAsync(int count); // Thêm hàm lấy sách bán chạy nhất
        Task UpdateBookQuantityAsync(int bookId, int quantity);

    }
}
