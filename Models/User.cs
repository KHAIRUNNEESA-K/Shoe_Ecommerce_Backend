using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONSTEPS_API.Models;

public partial class User
{
    [Key]
    [Column("User_Id")]
    public int UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Password { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Role { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
