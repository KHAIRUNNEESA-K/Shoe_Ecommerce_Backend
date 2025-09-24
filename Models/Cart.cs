using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONSTEPS_API.Models;

[Table("Cart")]
public partial class Cart
{
    [Key]
    [Column("Cart_Id")]
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AddedAt { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Carts")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User? User { get; set; }
}
