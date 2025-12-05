using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Implementations
{
    // Note: Assumes HRSystemContext is your main DbContext
    public class BookRepository : GenericRepository<BOOK>, IBookRepository
    {
        // _context is inherited from GenericRepository

        public BookRepository(HRSystemContext context) : base(context)
        {
        }

        // =======================================================
        // Check if ISBN already exists
        // =======================================================
        public async Task<bool> IsIsbnUniqueAsync(string isbn)
        {
            return !await _dbSet.AnyAsync(b => b.isbn == isbn);
            
        }

        // =======================================================
        // Get Book with Details (Includes Category for Read DTO)
        // =======================================================
        public async Task<BOOK> GetBookWithDetailsByIdAsync(int bookId)
        {
            return await _dbSet
                .Include(b => b.category) // Include the Category entity
                .FirstOrDefaultAsync(b => b.book_id == bookId);
        }

        // =======================================================
        // Search Books by Title Keyword
        // =======================================================
        public async Task<IEnumerable<BOOK>> GetBooksByTitleAsync(string titleKeyword)
        {
            // Case-insensitive search using Contains
            return await _dbSet
                .Include(b => b.category)
                .Where(b => b.title.Contains(titleKeyword))
                .ToListAsync();
        }

        // =======================================================
        // Get Books by Category ID (Required for filtering)
        // =======================================================
        public async Task<IEnumerable<BOOK>> GetBooksByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(b => b.category)
                .Where(b => b.category_id == categoryId)
                .ToListAsync();
        }

        // ⚠️ ملاحظة: يمكنك تجاوز (Override) دالة GetAllAsync() هنا إذا كنت تريد تضمين Category بشكل دائم في جميع عمليات الـ GET.
        /*
        public override async Task<IEnumerable<BOOK>> GetAllAsync()
        {
            return await _dbSet.Include(b => b.category).ToListAsync();
        }
        */
    }
}