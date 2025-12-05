// HRSystem.BaseLibrary.DTOs/LibrarianDTOs.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output) 
    // =========================================================================
    public class LibrarianReadDto
    {
        public int LibrarianId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; }
       
    }

    // =========================================================================
    // 2. CREATE DTO (Input) 
    // =========================================================================
    public class LibrarianCreateDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone]
        [StringLength(11)]
        public string Phone { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public int BranchId { get; set; }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
    }

    // =========================================================================
    // 3. UPDATE DTO (Input)
    // =========================================================================
    public class LibrarianUpdateDto
    {
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [Phone]
        [StringLength(11)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public int BranchId { get; set; }

        public decimal? Salary { get; set; }
    }
}