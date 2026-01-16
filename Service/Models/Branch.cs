using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string? Address { get; set; }

    public int? Manager { get; set; }

    public int? Phone { get; set; }

    public int Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
