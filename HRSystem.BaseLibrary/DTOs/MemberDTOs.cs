// HRSystem.BaseLibrary.DTOs/MemberDTOs.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output) - Used for displaying member details
    // =========================================================================
    public class MemberReadDto
    {
        public int MemberId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string MembershipType { get; set; }
        public string Status { get; set; }
    }

    // =========================================================================
    // 2. CREATE DTO (Input) - Used for new member registration (POST /register)
    // =========================================================================
    public class MemberCreateDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(50)]
        public string MembershipType { get; set; } = "Standard";
    }

    // =========================================================================
    // 3. UPDATE DTO (Input) - Used for modifying member details (PUT /{id})
    // =========================================================================
    public class MemberUpdateDto
    {
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string? MembershipType { get; set; }

        // Status updates should generally be restricted to Admin/Librarian
        [StringLength(50)]
        public string? Status { get; set; }
    }
}