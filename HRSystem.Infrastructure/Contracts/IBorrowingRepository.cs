using HRSystem.BaseLibrary.Models;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    public interface IBorrowingRepository : IGenericRepository<BORROWING>
    {
        Task<int> CountActiveBorrowingsAsync(int memberId);
        Task<bool> HasUnpaidFinesAsync(int memberId);
        Task<BORROWING> GetBorrowingWithBookAsync(int borrowingId);
    }
}
