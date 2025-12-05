using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRSystem.BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;


[Table("RESERVATION")]
public partial class RESERVATION
{
    [Key]
    public int reservation_id { get; set; }

    public int member_id { get; set; }

    public int book_id { get; set; }

    public DateOnly reservation_date { get; set; }

    public DateOnly arrive_date { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string status { get; set; }

    public int priority_number { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [ForeignKey("book_id")]
    [InverseProperty("RESERVATIONs")]
    public virtual BOOK book { get; set; } = null;

    [ForeignKey("member_id")]
    [InverseProperty("RESERVATIONs")]
    public virtual MEMBER member { get; set; }
}
