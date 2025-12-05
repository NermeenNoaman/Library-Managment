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
    public int generated_by { get; set; }

    [Required]
    public int TotalBooks { get; set; }

    [Required]
    public int TotalCategories { get; set; }

    public int TotalMembers { get; set; }

    public int TotalBorrowingCount { get; set; }

    public int TotalActiveBorrowings { get; set; }


   

    [ForeignKey("generated_by")]
    [InverseProperty("REPORTs")]
    public virtual LIBRARIAN generated_byNavigation { get; set; }
}
