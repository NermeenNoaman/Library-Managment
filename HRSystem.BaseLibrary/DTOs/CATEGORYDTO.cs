using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.BaseLibrary.DTOs
{
   

    public class CategoryCreateUpdateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

        public class CategoryReadDto
        {
            public int CategoryId { get; set; }

            public string CategoryName { get; set; } = string.Empty;

            public string? Description { get; set; }

            public DateTime? CreatedAt { get; set; }

            public DateTime? UpdatedAt { get; set; }
        }


    }

