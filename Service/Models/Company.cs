using System;
using System.Collections.Generic;

namespace Service.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Tagline { get; set; }

    public long Phone { get; set; }

    public long? AlternatePhone { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? Facebook { get; set; }

    public string? Instagram { get; set; }

    public string? Youtube { get; set; }

    public string? Whatsapp { get; set; }

    public string? GstNumber { get; set; }

    public string? PanNumber { get; set; }

    public string? CinNumber { get; set; }

    public string? BankName { get; set; }

    public string? AccountNumber { get; set; }

    public string? AccountHolderName { get; set; }

    public string? IfscCode { get; set; }

    public string? BranchName { get; set; }

    public string? UpiId { get; set; }

    public string? InvoicePrefix { get; set; }

    public int? InvoiceStartNumber { get; set; }

    public string? TermsConditions { get; set; }

    public string? FooterNote { get; set; }

    public string? LogonName { get; set; }

    public string? LogoPath { get; set; }

    public string? SignaturenName { get; set; }

    public string? SignaturePath { get; set; }

    public int Status { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; } = new List<CompanyAddress>();
}
