using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models; // Include Entities

public class ReportRepository : IReportRepository
{
    private readonly HRSystemContext _context;

    public ReportRepository(HRSystemContext context)
    {
        _context = context;
    }

    // =======================================================
    // Total Count for any Entity (Books, Members, Categories)
    // =======================================================
    public async Task<int> GetTotalCountAsync<T>() where T : class
    {
        return await _context.Set<T>().CountAsync();
    }

    // =======================================================
    // Total Active Borrowings
    // =======================================================
    public async Task<int> GetActiveBorrowingCountAsync()
    {
        return await _context.BORROWINGs.CountAsync(b => b.status == "Borrowed");
    }

    

   
}