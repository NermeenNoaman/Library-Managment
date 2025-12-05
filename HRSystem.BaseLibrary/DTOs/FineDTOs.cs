// HRSystem.BaseLibrary.DTOs/FineDTOs.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output) 
    // =========================================================================
    public class FineReadDto
    {
        public int FineId { get; set; }
        public int MemberId { get; set; }
        public int BorrowingId { get; set; }
        public decimal FineAmount { get; set; }
        public DateTime FineDate { get; set; }
        public string PaymentStatus { get; set; } 
        public DateTime? PaymentDate { get; set; }
    }

    // =========================================================================
    // 2. PAY DTO (Input) 
    // =========================================================================
    public class FinePayDto
    {
        [Required(ErrorMessage = "Fine ID is required.")]
        public int FineId { get; set; }

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "Payment amount must be positive.")]
        public decimal PaymentAmount { get; set; }
    }

}