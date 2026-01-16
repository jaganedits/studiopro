using Newtonsoft.Json.Linq;
using Service.Utils;

namespace Service.Services.Admin.Branch;

public interface IBranchService
{
    Task<CustomResponse> PageInit (JObject obj);
    Task<CustomResponse> ListInit (JObject obj);
    Task<CustomResponse> Create (JObject obj);
    Task<CustomResponse> Update (JObject obj);
    Task<CustomResponse> ChangeStatus (JObject obj);
}