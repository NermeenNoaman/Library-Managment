// HRSystem.Infrastructure.Contracts/IReservationService.cs

using HRSystem.BaseLibrary.DTOs;
using System.Threading.Tasks;

public interface IReservationService
{
    // Main logic for creating the reservation, calculating priority and arrival date
    Task<ReservationReadDto> CreateReservationAsync(int memberId, int bookId);

    // Logic to check and transfer the reservation to borrowing (when the book arrives)
    Task<bool> ProcessBookArrivalAsync(int bookId);

    // Get user's active reservations
    Task<IEnumerable<ReservationReadDto>> GetMyReservationsAsync(int memberId);
    Task<ReservationReadDto> UpdateReservationStatusAsync(int reservationId, ReservationUpdateDto dto);
}