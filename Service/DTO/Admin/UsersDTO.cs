using Service.Models;

namespace Service.DTO.Users;

public class UsersDTO
{
    public User users { get; set; }
    
    public List<Role> rolesList { get; set; }
    
    public List<SystemMetadatum> statusList { get; set; }
}