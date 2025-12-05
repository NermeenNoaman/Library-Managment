using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    public interface IReportRepository
    {
        // Total Counts
        Task<int> GetTotalCountAsync<T>() where T : class;

        // Borrowing/Reservation Counts
        Task<int> GetActiveBorrowingCountAsync();

    }
}
