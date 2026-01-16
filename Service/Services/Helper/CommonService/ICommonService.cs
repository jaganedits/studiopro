using Newtonsoft.Json.Linq;

namespace Service.Services.Helper.CommonService;

public interface ICommonService
{
    Task<string> GenerateUniqueCode(int Documentid);
    Task<string> ProcessToken(JObject obj);
}