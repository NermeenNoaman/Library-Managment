using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("REPORT")]
public partial class REPORT
{
    [Key]
    public int report_id { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string report_type { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime generated_date { get; set; }

    public int generated_by { get; set; }

    public string report_data { get; set; }

    [Column(TypeName = "text")]
    public string filters_applied { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [ForeignKey("generated_by")]
    [InverseProperty("REPORTs")]
    public virtual LIBRARIAN generated_byNavigation { get; set; }
}
