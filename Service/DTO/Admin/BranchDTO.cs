using Service.CustomModels.Admin.Branch;
using Service.Models;

namespace Service.DTO.Users;

public class BranchDTO
{
    public List<SystemMetadatum> StatusList { get; set; }
    public Branch branch { get; set; }
    public List<User> UsersList { get; set; }
    public List<BranchListInit> BranchListInitList { get; set; }
}