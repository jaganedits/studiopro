using Newtonsoft.Json.Linq;
using Service.Utils;

namespace Service.Services.Admin.Company;

public interface ICompanyService
{
    Task<CustomResponse> PageInit(JObject obj);
    Task<CustomResponse> Update(JObject obj, IFormFile? logo, IFormFile? signature);
    Task<CustomResponse> CreateAddress(JObject obj);
    Task<CustomResponse> UpdateAddress(JObject obj);
    Task<CustomResponse> ChangeStatus(JObject obj);
}