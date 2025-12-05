using System;
using System.Collections.Generic;
using HRSystem.BaseLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Index("email", Name = "UQ__USERs__AB6E6164BF49B0D4", IsUnique = true)]
public partial class USER
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int user_id { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string email { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string password { get; set; }

    [Required]
    [StringLength(200)]
    [Unicode(false)]
    public string fullname { get; set; }

    [Required]
    [StringLength(11)]
    [Unicode(false)]
    public string phone { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string role { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [InverseProperty("user")]
    public virtual MEMBER MEMBER { get; set; }

    [InverseProperty("user")]
    public virtual ICollection<REFRESH_TOKEN> REFRESH_TOKENs { get; set; } = new List<REFRESH_TOKEN>();
}