using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using System.Linq; // Required for Any() check if GetByNameAsync returns IQueryable

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ReportController : ControllerBase
{
    private readonly IReportRepository _repo;

    public ReportController(IReportRepository repo)
    {
        _repo = repo;
    }

    // =========================================================================
    // GET: Dashboard Summary (Sequential Aggregation)
    // =========================================================================
    [HttpGet("dashboard-summary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DashboardReportDto))]
    public async Task<IActionResult> GetDashboardSummary()
    {

        // Count Totals:
        var totalBooks = await _repo.GetTotalCountAsync<BOOK>();
        var totalMembers = await _repo.GetTotalCountAsync<MEMBER>();
        var totalCategories = await _repo.GetTotalCountAsync<CATEGORY>();

        // Count Borrowing/Active States:
        var totalBorrowings = await _repo.GetTotalCountAsync<BORROWING>();
        var totalActiveBorrowings = await _repo.GetActiveBorrowingCountAsync();

        var totalActiveReservations = 0; 
        var totalUnpaidFines = 0.0m;    

        var report = new DashboardReportDto
        {
            TotalBooks = totalBooks,
            TotalMembers = totalMembers,
            TotalCategories = totalCategories,
            TotalBorrowingCount = totalBorrowings,
            TotalActiveBorrowings = totalActiveBorrowings,
           
        };

        return Ok(report);
    }
}