using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONSTEPS_API.Models;

public partial class Product
{
    [Key]
    [Column("Product_Id")]
    public int ProductId { get; set; }

    [Column("Product_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string ProductName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Column("Image_Url")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    public int? CategoryById { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    public int? Size { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [ForeignKey("CategoryById")]
    [InverseProperty("Products")]
    public virtual Category? CategoryBy { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
