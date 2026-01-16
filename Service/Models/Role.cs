using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public int CompanyId { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public int Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
