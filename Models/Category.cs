using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONSTEPS_API.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("Category_Id")]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("CategoryBy")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
