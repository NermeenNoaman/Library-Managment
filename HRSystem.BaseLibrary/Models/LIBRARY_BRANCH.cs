using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("LIBRARY_BRANCH")]
[Index("branch_name", Name = "UQ__LIBRARY___CF7A7E6C3D8F47FC", IsUnique = true)]
public partial class LIBRARY_BRANCH
{
    [Key]
    public int branch_id { get; set; }

    public int library_id { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string branch_name { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string branch_location { get; set; }

    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string branch_phone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [ForeignKey("library_id")]
    [InverseProperty("LIBRARY_BRANCHes")]
    public virtual LIBRARIAN library { get; set; }

    [ForeignKey("library_id")]
    [InverseProperty("LIBRARY_BRANCHes")]
    public virtual LIBRARY libraryNavigation { get; set; }
}
