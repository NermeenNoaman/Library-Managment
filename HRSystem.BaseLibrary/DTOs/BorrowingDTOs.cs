using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output)
    // =========================================================================
    public class BorrowingReadDto
    {
        public int BorrowingId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
    }

    // =========================================================================
    // 2. CREATE DTO (Input)
    // =========================================================================
    public class BorrowingCreateDto
    {
        [Required(ErrorMessage = "Member ID is required.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; }
    }

    // =========================================================================
    // 3. UPDATE DTO (Input)
    // =========================================================================
    public class BorrowingUpdateDto
    {
        [Required(ErrorMessage = "Borrowing ID is required for update.")]
        public int BorrowingId { get; set; }

        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [StringLength(70)]
        public string? Status { get; set; }

        public int? MemberId { get; set; }
        public int? BookId { get; set; }
    }
}
