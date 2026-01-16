using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class CompanyAddress
{
    public int CompanyAddressId { get; set; }

    public int CompanyId { get; set; }

    public string Label { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Area { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public string? Landmark { get; set; }

    public bool IsPrimary { get; set; }

    public int Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Company Company { get; set; } = null!;
}
