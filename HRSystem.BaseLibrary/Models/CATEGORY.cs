using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using HRSystem.BaseLibrary.Models;


namespace HRSystem.BaseLibrary.Models;

[Table("CATEGORY")]
[Index("category_name", Name = "UQ__CATEGORY__5189E2557ADC450D", IsUnique = true)]
public partial class CATEGORY
{
    [Key]
    public int category_id { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string category_name { get; set; }

    [Column(TypeName = "text")]
    public string description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("category")]
    public virtual ICollection<BOOK> BOOKs { get; set; } = new List<BOOK>();
}
