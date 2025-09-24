using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONSTEPS_API.Models;

public partial class Wishlist
{
    [Key]
    [Column("Wishlist_Id")]
    public int WishlistId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Wishlists")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Wishlists")]
    public virtual User? User { get; set; }
}
