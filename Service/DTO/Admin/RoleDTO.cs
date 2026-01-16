

using Service.CustomModels.Admin.Role;
using Service.Models;

namespace Service.DTO.Users;

public class RoleDTO
{
    public List<SystemMetadatum> StatusList { get; set; }
    public Role role { get; set; }
    public List<RoleList>  RoleList { get; set; }
    
}