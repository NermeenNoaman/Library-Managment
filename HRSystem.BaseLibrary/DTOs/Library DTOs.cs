using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output)
    // =========================================================================
    public class LibraryReadDto
    {
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
        public string TaxNumber { get; set; }
        public string LibraryEmail { get; set; } 
        public string LibraryPhone { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    // =========================================================================
    // 2. CREATE DTO (Input)
    // =========================================================================
    public class LibraryCreateDto
    {
        [Required(ErrorMessage = "Library name is required.")]
        [StringLength(255)]
        public string LibraryName { get; set; }

        [Required(ErrorMessage = "Tax number is required.")]
        [StringLength(50)]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(255)]
        public string LibraryEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        [StringLength(20)]
        public string LibraryPhone { get; set; }
    }

    // =========================================================================
    // 3. UPDATE DTO (Input)
    // =========================================================================
    public class LibraryUpdateDto
    {
        [Required(ErrorMessage = "Library ID is required for update.")]
        public int LibraryId { get; set; }

        [StringLength(255)]
        public string? LibraryName { get; set; }

        [StringLength(50)]
        public string? TaxNumber { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? LibraryEmail { get; set; }

        [StringLength(20)]
        public string? LibraryPhone { get; set; }
    }
}