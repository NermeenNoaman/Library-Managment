using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.BaseLibrary.Models;

[Table("REFRESH_TOKEN")]
[Index("token", Name = "UQ__REFRESH___CA90DA7AC69B8B45", IsUnique = true)]
public partial class REFRESH_TOKEN
{
    [Key]
    public int token_id { get; set; }

    public int user_id { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string email { get; set; }

    [Required]
    [StringLength(500)]
    [Unicode(false)]
    public string token { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime expires { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created { get; set; }

    public DateTime revoked { get; set; }

    [ForeignKey("user_id")]
    [InverseProperty("REFRESH_TOKENs")]
    public virtual USER user { get; set; }
}
