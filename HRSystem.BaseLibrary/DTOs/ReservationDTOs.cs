// HRSystem.BaseLibrary.DTOs/ReservationDTOs.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output) - Used for displaying reservation details
    // =========================================================================
    public class ReservationReadDto
    {
        public int ReservationId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateOnly ReservationDate { get; set; }
        public DateOnly ArriveDate { get; set; } // Estimated arrival date
        public string Status { get; set; }
        public int PriorityNumber { get; set; }
    }

    // =========================================================================
    // 2. CREATE DTO (Input) - Used by the Controller
    // =========================================================================
    // Note: MemberId and BookId are passed via Controller/Route, so this DTO is simple
    public class ReservationCreateDto
    {
        // This DTO can be empty since all data comes from the token/route.
        // We define it mainly for architectural consistency.
    }

    // =========================================================================
    // 3. UPDATE DTO (Input) - Used by Librarian to change status
    // =========================================================================
    public class ReservationUpdateDto
    {
        [Required(ErrorMessage = "Status is required for updating reservation.")]
        [StringLength(50)]
        public string Status { get; set; }

    }
}