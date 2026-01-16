namespace Service.CustomModels.Admin.Role;

public class RoleList
{
    public int RoleId { get; set; }

    public int CompanyId { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public int Status { get; set; }
    
    public string StatusName { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}