using Newtonsoft.Json.Linq;
using Service.Utils;

namespace Service.Services.Admin.Users;

public interface IUsersService
{
    Task<CustomResponse> PageInit (JObject obj);
    Task<CustomResponse> ListInit (JObject obj);
}