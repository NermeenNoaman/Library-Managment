using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("MEMBER")]
[Index("user_id", Name = "UQ__MEMBER__B9BE370EF0BA78E6", IsUnique = true)]
public partial class MEMBER
{
    [Key]
    public int member_id { get; set; }

    public int user_id { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string email { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string first_name { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string last_name { get; set; }

    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string phone { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string address { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime date_of_birth { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime registration_date { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string membership_type { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("member")]
    public virtual ICollection<BORROWING> BORROWINGs { get; set; } = new List<BORROWING>();

    [InverseProperty("member")]
    public virtual ICollection<FINE> FINEs { get; set; } = new List<FINE>();

    [InverseProperty("member")]
    public virtual ICollection<RESERVATION> RESERVATIONs { get; set; } = new List<RESERVATION>();

    [ForeignKey("user_id")]
    [InverseProperty("MEMBER")]
    public virtual USER user { get; set; }
}
