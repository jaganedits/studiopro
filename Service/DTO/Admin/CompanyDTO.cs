using Service.Models;

namespace Service.DTO.Users;

public class CompanyDTO
{
    public Company? Company { get; set; }
    public List<CompanyAddress>? AddressList { get; set; }
    public CompanyAddress? Address { get; set; }
    public List<SystemMetadatum>? StatusList { get; set; }
}