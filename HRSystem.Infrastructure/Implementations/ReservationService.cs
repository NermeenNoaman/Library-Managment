// HRSystem.Infrastructure.Implementations.Services/ReservationService.cs

using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repo;
    private readonly IBookRepository _bookRepo; // Needed to check copies
    private readonly IMapper _mapper;
    private readonly HRSystemContext _context;

    public ReservationService(IReservationRepository repo, IBookRepository bookRepo, IMapper mapper, HRSystemContext context)
    {
        _repo = repo;
        _bookRepo = bookRepo;
        _mapper = mapper;
        _context = context;
    }

    // =======================================================
    // 1. Create Reservation
    // =======================================================
    public async Task<ReservationReadDto> CreateReservationAsync(int memberId, int bookId)
    {
        // 1. Validation Checks
        var book = await _bookRepo.GetByIdAsync(bookId);
        if (book == null)
            throw new ArgumentException("Book not found.");

        if (book.available_copies > 0)
            throw new InvalidOperationException("Book is currently available for direct borrowing.");

        if (await _repo.IsBookAlreadyReservedByUserAsync(memberId, bookId))
            throw new InvalidOperationException("Member already has an active reservation for this book.");

        // 2. Calculate Priority and Arrival Date
        var highestPriority = await _repo.GetHighestPriorityAsync(bookId);
        var earliestDueDate = await _repo.GetEarliestBookDueDateAsync(bookId);

        // 3. Estimate Arrival Date: Use the earliest due date + 7 days for every active reservation ahead of this one.
        DateTime arriveDate;
        if (!earliestDueDate.HasValue)
        {
            // Should not happen if available_copies = 0, but safety check.
            arriveDate = DateTime.Today.AddDays(7);
        }
        else
        {
            // Base time is the earliest expected return time
            DateTime baseDate = earliestDueDate.Value.Date;

            // Add 7 days for the first reservation, plus 7 days for every reservation currently ahead
            // (Assumes 7 days borrowing period and 1 reservation per copy)
            int daysToAdd = 7 * (highestPriority + 1); // +1 because the new reservation takes the next slot

            arriveDate = baseDate.AddDays(daysToAdd);
        }

        // 4. Create Entity
        var reservation = new RESERVATION
        {
            member_id = memberId,
            book_id = bookId,
            reservation_date = DateOnly.FromDateTime(DateTime.UtcNow),
            arrive_date = DateOnly.FromDateTime(arriveDate),
            status = "Pending",
            priority_number = highestPriority + 1,
            created_at = DateTime.UtcNow
        };

        // 5. Save and Return
        await _repo.AddAsync(reservation);
        await _repo.SaveChangesAsync();

        return _mapper.Map<ReservationReadDto>(reservation);
    }

    // (Other service methods like GetMyReservationsAsync, ProcessBookArrivalAsync should be implemented here)
    // ...

    public async Task<IEnumerable<ReservationReadDto>> GetMyReservationsAsync(int memberId)
    {
        var entities = await _repo.GetReservationsForMemberAsync(memberId);

        return _mapper.Map<IEnumerable<ReservationReadDto>>(entities);
    }

    public Task<bool> ProcessBookArrivalAsync(int bookId)
    {
        // This complex logic (checking priority 1 reservation, notifying the member, and starting a holding period)
        // is typically implemented here. For now, it's a placeholder.
        throw new NotImplementedException();
    }

    public async Task<ReservationReadDto> UpdateReservationStatusAsync(int reservationId, ReservationUpdateDto dto)
    {
        var reservation = await _repo.GetByIdAsync(reservationId);

        if (reservation == null)
        {
            throw new InvalidOperationException("Reservation not found.");
        }

        if (reservation.status == "Fulfilled" || reservation.status == "Cancelled")
        {
            throw new InvalidOperationException($"Cannot update reservation status from '{reservation.status}'.");
        }

        reservation.status = dto.Status;
        reservation.updated_at = DateTime.UtcNow;

        

        _repo.UpdateAsync(reservation); 
        await _repo.SaveChangesAsync();

        return _mapper.Map<ReservationReadDto>(reservation);
    }
}