// HRSystem.Infrastructure.Contracts/IReservationRepository.cs

using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReservationRepository : IGenericRepository<RESERVATION>
{
    // Get the highest priority number for a specific book
    Task<int> GetHighestPriorityAsync(int bookId);

    // Get the due date of the earliest active borrowing for a book
    Task<DateTime?> GetEarliestBookDueDateAsync(int bookId);

    // Check if the member already has an active reservation for this book
    Task<bool> IsBookAlreadyReservedByUserAsync(int memberId, int bookId);

    // Get active reservations for a book, ordered by priority
    Task<IEnumerable<RESERVATION>> GetActiveReservationsByBookIdAsync(int bookId);
    Task<IEnumerable<RESERVATION>> GetReservationsForMemberAsync(int memberId);
}