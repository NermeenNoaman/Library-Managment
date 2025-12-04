using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("FINE")]
public partial class FINE
{
    [Key]
    public int fine_id { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal fine_amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime fine_date { get; set; }

    [Required]
    [StringLength(50)]
    public string payment_status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime payment_date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    public int member_id { get; set; }

    public int borrowing_id { get; set; }

    [ForeignKey("borrowing_id")]
    [InverseProperty("FINEs")]
    public virtual BORROWING borrowing { get; set; }

    [ForeignKey("member_id")]
    [InverseProperty("FINEs")]
    public virtual MEMBER member { get; set; }
}
