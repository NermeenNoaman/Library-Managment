// HRSystem.Infrastructure.Implementations/ReservationRepository.cs

using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReservationRepository : GenericRepository<RESERVATION>, IReservationRepository
{
    private readonly HRSystemContext _context;

    public ReservationRepository(HRSystemContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RESERVATION>> GetReservationsForMemberAsync(int memberId)
    {
        return await _dbSet
            .Where(r => r.member_id == memberId)
            .OrderBy(r => r.status == "Pending" ? 0 : 1)
            .ThenBy(r => r.arrive_date)
            .ToListAsync();
    }

    // --- Specific Repository Queries ---

    public async Task<int> GetHighestPriorityAsync(int bookId)
    {
        // Find the maximum priority number for this book, default to 0 if none exist
        var maxPriority = await _dbSet
          .Where(r => r.book_id == bookId && r.status == "Pending")
          .MaxAsync(r => (int?)r.priority_number);

        return maxPriority.GetValueOrDefault(0);
    }

    public async Task<DateTime?> GetEarliestBookDueDateAsync(int bookId)
    {
        // Get the earliest DUE DATE of active borrowings for this book
        return await _context.BORROWINGs
            .Where(b => b.book_id == bookId && b.status == "Borrowed")
            .OrderBy(b => b.due_date) // Get the earliest one
            .Select(b => b.due_date)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsBookAlreadyReservedByUserAsync(int memberId, int bookId)
    {
        // Check if a member has any active (Pending) reservation for this book
        return await _dbSet
            .AnyAsync(r => r.member_id == memberId && r.book_id == bookId && r.status == "Pending");
    }

    public async Task<IEnumerable<RESERVATION>> GetActiveReservationsByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Where(r => r.book_id == bookId && r.status == "Pending")
            .OrderBy(r => r.priority_number)
            .ToListAsync();
    }
}