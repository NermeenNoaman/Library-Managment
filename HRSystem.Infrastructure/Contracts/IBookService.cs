using HRSystem.BaseLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    public interface IBookService
    {
        Task<BookReadDto> CreateBookAsync(BookCreateDto dto);
        Task<IEnumerable<BookReadDto>> GetAllBooksAsync();
        Task<BookReadDto> GetBookByIdAsync(int id);
        Task<bool> UpdateBookAsync(int id, BookUpdateDto dto);
        Task<bool> DeleteBookAsync(int id);

        Task<IEnumerable<BookReadDto>> SearchBooksByTitleAsync(string titleKeyword);
        Task<IEnumerable<BookReadDto>> GetBooksByCategoryIdAsync(int categoryId);
    }
}
