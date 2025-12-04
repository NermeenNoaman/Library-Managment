using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.BaseLibrary.DTOs
{
    // =========================================================================
    // 1. READ DTO (Output)
    // =========================================================================
    public class LibraryBranchReadDto
    {
        public int BranchId { get; set; }
        public int LibraryId { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }
        public string BranchPhone { get; set; }
        public DateTime? CreatedAt { get; set; }
        // Optional: public string LibraryName { get; set; } // Requires joining
    }

    // =========================================================================
    // 2. CREATE DTO (Input)
    // =========================================================================
    public class LibraryBranchCreateDto
    {
        [Required(ErrorMessage = "Library ID is required to link the branch.")]
        public int LibraryId { get; set; }

        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(255)]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Branch location is required.")]
        [StringLength(255)]
        public string BranchLocation { get; set; }

        [Required(ErrorMessage = "Branch phone number is required.")]
        [Phone]
        [StringLength(20)]
        public string BranchPhone { get; set; }
    }

    // =========================================================================
    // 3. UPDATE DTO (Input)
    // =========================================================================
    public class LibraryBranchUpdateDto
    {
        [Required(ErrorMessage = "Branch ID is required for update.")]
        public int BranchId { get; set; }

        public int? LibraryId { get; set; } // Allowing optional update of Library Link

        [StringLength(255)]
        public string? BranchName { get; set; }

        [StringLength(255)]
        public string? BranchLocation { get; set; }

        [StringLength(20)]
        public string? BranchPhone { get; set; }
    }
}