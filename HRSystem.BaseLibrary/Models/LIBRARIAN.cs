using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("LIBRARIAN")]
public partial class LIBRARIAN
{
    [Key]
    public int librarian_id { get; set; }

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

    [Required]
    public int branch_id { get; set; }


   

    [InverseProperty("library")]
    public virtual ICollection<LIBRARY_BRANCH> LIBRARY_BRANCHes { get; set; } = new List<LIBRARY_BRANCH>();

    [InverseProperty("generated_byNavigation")]
    public virtual ICollection<REPORT> REPORTs { get; set; } = new List<REPORT>();

    
}
