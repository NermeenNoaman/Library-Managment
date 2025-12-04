using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("LIBRARY")]
[Index("library_email", Name = "UQ__LIBRARY__754A0AA589EA2E34", IsUnique = true)]
[Index("tax_number", Name = "UQ__LIBRARY__8A87F6312EE4E392", IsUnique = true)]
public partial class LIBRARY
{
    [Key]
    public int library_id { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string library_name { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string tax_number { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string library_email { get; set; }

    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string library_phone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("libraryNavigation")]
    public virtual ICollection<LIBRARY_BRANCH> LIBRARY_BRANCHes { get; set; } = new List<LIBRARY_BRANCH>();
}
