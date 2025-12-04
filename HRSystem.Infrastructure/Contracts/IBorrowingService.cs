using HRSystem.BaseLibrary.Models;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.DTOs;
namespace HRSystem.Infrastructure.Contracts
{
    public interface IBorrowingService
    {
        Task<BorrowingReadDto> BorrowBookAsync(BorrowingCreateDto dto);
        Task<BorrowingReadDto> ReturnBookAsync(int borrowingId);
    }
}
