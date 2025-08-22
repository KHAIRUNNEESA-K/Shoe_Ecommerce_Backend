using System;
using System.Collections.Generic;

namespace ONSTEPS_API.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PhoneNo { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }
}
