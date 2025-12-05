using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace HRSystem.Infrastructure.Repositories
{
    public class BorrowingRepository : GenericRepository<BORROWING>, IBorrowingRepository
    {

        public BorrowingRepository(HRSystemContext context) : base(context) { }

        // =======================================================
        // Get Borrowings by Member ID (Active Only) - IMPLEMENTATION
        // =======================================================
        public async Task<IEnumerable<BORROWING>> GetBorrowingsByMemberIdAsync(int memberId)
        {
            
            return await _dbSet
                .Where(b => b.member_id == memberId && b.status == "Borrowed")
                .Include(b => b.book)
                .ToListAsync();
        }
        public async Task<bool> IsBookCurrentlyBorrowedByMemberAsync(int memberId, int bookId)
        {
            return await _dbSet
                             .AnyAsync(b => b.member_id == memberId &&
                                            b.book_id == bookId &&
                                            b.return_date == null);
        }

        public async Task<int> CountActiveBorrowingsAsync(int memberId)
        {
            return await _dbSet.CountAsync(b => b.member_id == memberId && b.status == "Borrowed");
        }

        public async Task<bool> HasUnpaidFinesAsync(int memberId)
        {
            return await _context.Set<FINE>()
                                 .AnyAsync(f => f.member_id == memberId && f.payment_status == "Unpaid");
        }

        public async Task<BORROWING> GetBorrowingWithBookAsync(int borrowingId)
        {
            var borrowing = await _dbSet.Include(b => b.book)
            .FirstOrDefaultAsync(b => b.borrowing_id == borrowingId);
            return borrowing ?? throw new InvalidOperationException("Borrowing not found.");
        }
    }
}
