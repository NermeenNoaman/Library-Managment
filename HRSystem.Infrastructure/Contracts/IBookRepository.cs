using HRSystem.BaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    public interface IBookRepository : IGenericRepository<BOOK>
    {
        Task<IEnumerable<BOOK>> GetBooksByTitleAsync(string titleKeyword);

        Task<BOOK> GetBookWithDetailsByIdAsync(int bookId);

        Task<IEnumerable<BOOK>> GetBooksByCategoryIdAsync(int categoryId);
        Task<bool> IsIsbnUniqueAsync(string isbn);
    }
}
