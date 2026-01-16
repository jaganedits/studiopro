using Newtonsoft.Json.Linq;
using Service.ContextHelpers;
using Service.CustomModels.Admin.Role;
using Service.DTO.Users;
using Service.Models;
using Service.Services.Helper.CommonService;
using Service.UnitOfWork.Uow;
using Service.Utils;
using Service.Validation.Admin;

namespace Service.Services.Admin.Role;

public class RolesService : IRolesService
{
    private readonly IUowProvider uowProvider;
    private readonly IDapperContext dapperContext;
    private CustomResponse res = new CustomResponse();
    private readonly ICommonService commonService;
    private RoleDTO roleDTO = new RoleDTO();

    public RolesService(
        IUowProvider _uowProvider,
        IDapperContext _dapperContext,
        ICommonService _commonService
    )
    {
        uowProvider = _uowProvider;
        dapperContext = _dapperContext;
        commonService = _commonService;
    }

    private RoleValidation validation = new RoleValidation();


    public async Task<CustomResponse> PageInit(JObject obj)
    {
        string type = "PageInit";
        int roleId = obj["RoleId"].ToObject<int>();
        int companyId = obj["CompanyId"].ToObject<int>();
        int branchId = obj["BranchId"].ToObject<int>();

        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_ROLE", new
                {
                    type,
                    roleId,
                    companyId
                });
                roleDTO.StatusList = (await spcall.ReadAsync<SystemMetadatum>()).ToList();
                if (roleId > 0)
                {
                    roleDTO.role = (await spcall.ReadAsync<Models.Role>()).SingleOrDefault();
                }
            }

            res.IsSuccess = true;
            res.Data = roleDTO;
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
        string type = "ListInit";
        int roleId = obj["RoleId"].ToObject<int>();
        int companyId = obj["CompanyId"].ToObject<int>();
        int branchId = obj["BranchId"].ToObject<int>();

        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_ROLE", new
                {
                    type,
                    roleId,
                    companyId
                });
                
                roleDTO.RoleList = (await spcall.ReadAsync<RoleList>()).ToList();
                
            }

            res.IsSuccess = true;
            res.Data = roleDTO;
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Title = "Error";
            res.Message = ex.Message;
        }

        return res;
    }

    public async Task<CustomResponse> Create(JObject obj)
    {
        Models.Role role = obj["Role"].ToObject<Models.Role>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var returnValidation = validation.Validations(role);
                if (returnValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = returnValidation.Message;
                    res.Data = roleDTO;
                    return res;
                }

                var roleRepo = uow.GetRepository<Models.Role>();
                var userRepo = uow.GetRepository<User>();

                var duplicateValidation = validation.ValidateDuplicateForCreate(role, roleRepo);
                if (duplicateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = duplicateValidation.Title;
                    res.Message = duplicateValidation.Message;
                    res.Data = roleDTO;
                    return res;
                }

                role.CreatedDate = DateTime.Now;
                role.RoleCode = await commonService.GenerateUniqueCode(9);
                roleRepo.Add(role);
                await uow.SaveChangesAsync();
            }

            res.IsSuccess = true;
            res.Title = "Success Message";
            res.Message = "Role created successfully";
            res.Data = roleDTO;
            return res;
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Title = "Error";
            res.Message = ex.Message;
        }

        return res;
    }

    public async Task<CustomResponse> Update(JObject obj)
    {
        Models.Role role = obj["Role"].ToObject<Models.Role>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var returnValidation = validation.Validations(role);
                if (returnValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = returnValidation.Message;
                    res.Data = roleDTO;
                    return res;
                }

                var roleRepo = uow.GetRepository<Models.Role>();

                var duplicateValidation = validation.ValidateDuplicateForUpdate(role, roleRepo);
                if (duplicateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = duplicateValidation.Title;
                    res.Message = duplicateValidation.Message;
                    res.Data = roleDTO;
                    return res;
                }

                // Check if status is being changed to inactive
                if (role.Status == 2)
                {
                    var userRepo = uow.GetRepository<User>();
                    var canInactivateValidation = validation.ValidateCanInactivate(role, userRepo);
                    if (canInactivateValidation.IsSuccess == false)
                    {
                        res.IsSuccess = false;
                        res.Title = canInactivateValidation.Title;
                        res.Message = canInactivateValidation.Message;
                        res.Data = roleDTO;
                        return res;
                    }
                }

                role.ModifiedDate = DateTime.Now;
                roleRepo.Update(role);
                await uow.SaveChangesAsync();
                res.IsSuccess = true;
                res.Title = "Sucess Message";
                res.Message = "Role details updated successfully";
                res.Data = roleDTO;
                return res;
            }
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Title = "Error";
            res.Message = ex.Message;
        }

        return res;
    }
    
    public async Task<CustomResponse> ChangeStatus(JObject obj)
    {
        int RoleId = obj["RoleId"].ToObject<int>();
        int UserId = obj["UserId"].ToObject<int>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var roleRepo = uow.GetRepository<Models.Role>();
                var userRepo = uow.GetRepository<User>();
                var role = (await roleRepo.QueryAsync(x => x.RoleId == RoleId)).SingleOrDefault();

                var canInactivateValidation = validation.ValidateCanInactivate(role, userRepo);
                if (canInactivateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = canInactivateValidation.Title;
                    res.Message = canInactivateValidation.Message;
                    res.Data = roleDTO;
                    return res;
                }

                role.Status = 2;
                role.ModifiedBy = UserId;
                role.ModifiedDate = DateTime.Now;
                roleRepo.Update(role);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Status Changed";
                res.Message = "Role has become Inactive";
                return res;
            }
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Title = "Error";
            res.Message = ex.Message;
            return res;
        }
    }
}