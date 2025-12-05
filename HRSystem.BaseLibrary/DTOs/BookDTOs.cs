using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.BaseLibrary.DTOs
{
    public class BookCreateDto
    {
        [Required]
        public string Isbn { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Publisher { get; set; }

        public int? PublicationYear { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public int Pages { get; set; }

        public string Description { get; set; }

    }

    public class BookUpdateDto
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int? CategoryId { get; set; }
        public string Publisher { get; set; }
        public int? PublicationYear { get; set; }

        [Range(1, int.MaxValue)]
        public int? TotalCopies { get; set; }
        public string Language { get; set; }
        public int? Pages { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class BookReadDto
    {
        public int BookId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string Publisher { get; set; }
        public int? PublicationYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string Language { get; set; }
        public int? Pages { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }


}
