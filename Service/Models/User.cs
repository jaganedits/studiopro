using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class User
{
    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? AvatarName { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? LastLogin { get; set; }

    public int Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
