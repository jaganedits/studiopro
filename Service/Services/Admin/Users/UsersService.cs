using Newtonsoft.Json.Linq;
using Service.ContextHelpers;
using Service.DTO.Users;
using Service.Models;
using Service.Services.Helper.CommonService;
using Service.UnitOfWork.Uow;
using Service.Utils;

namespace Service.Services.Admin.Users;

public class UsersService : IUsersService
{
    private readonly IUowProvider uowProvider;
    private readonly IDapperContext dapperContext;
    private CustomResponse res = new CustomResponse();
    private readonly ICommonService commonService;
    private UsersDTO dto = new UsersDTO();

    public UsersService(
        IUowProvider _uowProvider,
        IDapperContext _dapperContext,
        ICommonService _commonService
    )
    {
        uowProvider = _uowProvider;
        dapperContext = _dapperContext;
        commonService = _commonService;
    }

    public async Task<CustomResponse> PageInit(JObject obj)
    {
        string type = "PageInit";
        int userId = obj["UserId"].ToObject<int>();
        int companyId = obj["CompanyId"].ToObject<int>();
        int branchId = obj["BranchId"].ToObject<int>();
        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_ROLE", new
                {
                    type,
                    userId,
                    companyId
                });
                dto.statusList = (await spcall.ReadAsync<SystemMetadatum>()).ToList();
                dto.rolesList = (await spcall.ReadAsync<Models.Role>()).ToList();
            }

            res.IsSuccess = true;
            res.Data = dto;
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Title = "Error";
            res.Message = ex.Message;
        }

        return res;
    }

    public async Task<CustomResponse> ListInit(JObject obj)
    {
        throw new NotImplementedException();
    }
}