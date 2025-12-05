using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace HRSystem.BaseLibrary.Models;

[Table("BOOK")]
[Index("isbn", Name = "UQ__BOOK__99F9D0A4AE4CB329", IsUnique = true)]
public partial class BOOK
{
    [Key]
    public int book_id { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string isbn { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string title { get; set; }

    [Required]
    [StringLength(200)]
    [Unicode(false)]
    public string author_name { get; set; }

    public int category_id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string publisher { get; set; }

    public int? publication_year { get; set; }

    public int total_copies { get; set; }

    public int available_copies { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string language { get; set; }

    public int? pages { get; set; }

    [StringLength(200)]
    public string description { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("book")]
    public virtual ICollection<BORROWING> BORROWINGs { get; set; } = new List<BORROWING>();

    [InverseProperty("book")]
    public virtual ICollection<RESERVATION> RESERVATIONs { get; set; } = new List<RESERVATION>();

    [ForeignKey("category_id")]
    [InverseProperty("BOOKs")]
    public virtual CATEGORY category { get; set; }
}
