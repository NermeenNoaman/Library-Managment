using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("LIBRARIAN")]
[Index("user_id", Name = "UQ__LIBRARIA__B9BE370E0A0EDD19", IsUnique = true)]
public partial class LIBRARIAN
{
    [Key]
    public int librarian_id { get; set; }

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

    [Column(TypeName = "decimal(10, 2)")]
    public decimal salary { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime hire_date { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("librarian")]
    public virtual ICollection<BORROWING> BORROWINGs { get; set; } = new List<BORROWING>();

    [InverseProperty("library")]
    public virtual ICollection<LIBRARY_BRANCH> LIBRARY_BRANCHes { get; set; } = new List<LIBRARY_BRANCH>();

    [InverseProperty("generated_byNavigation")]
    public virtual ICollection<REPORT> REPORTs { get; set; } = new List<REPORT>();

    [ForeignKey("user_id")]
    [InverseProperty("LIBRARIAN")]
    public virtual USER user { get; set; }
}
