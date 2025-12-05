using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using HRSystem.BaseLibrary.Models;


namespace HRSystem.BaseLibrary.Models;

[Table("BORROWING")]
public partial class BORROWING
{
    [Key]
    public int borrowing_id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime borrow_date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime due_date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? return_date { get; set; }

    [Required]
    [StringLength(70)]
    public string status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    public int member_id { get; set; }

    public int book_id { get; set; }


    [InverseProperty("borrowing")]
    public virtual ICollection<FINE> FINEs { get; set; } = new List<FINE>();

    [ForeignKey("book_id")]
    [InverseProperty("BORROWINGs")]
    public virtual BOOK book { get; set; }

   

    [ForeignKey("member_id")]
    [InverseProperty("BORROWINGs")]
    public virtual MEMBER member { get; set; }
}
