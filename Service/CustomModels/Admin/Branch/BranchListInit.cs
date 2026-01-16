namespace Service.CustomModels.Admin.Branch;

public class BranchListInit
{
    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string? Address { get; set; }

    public string? ManagerName { get; set; }

    public int? Phone { get; set; }

    public int Status { get; set; }
    
    public string StatusName { get; set; }

    public int CreatedBy { get; set; }
    
    public string CreatedByName { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }
    
    public string ModifiedByName { get; set; }

    public DateTime? ModifiedDate { get; set; }
}