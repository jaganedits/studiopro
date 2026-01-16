using Newtonsoft.Json.Linq;
using Service.ContextHelpers;
using Service.CustomModels.Admin.Branch;
using Service.DTO.Users;
using Service.Models;
using Service.Services.Helper.CommonService;
using Service.UnitOfWork.Uow;
using Service.Utils;
using Service.Validation.Admin;

namespace Service.Services.Admin.Branch;

public class BranchService : IBranchService
{
    private readonly IUowProvider uowProvider;
    private readonly IDapperContext dapperContext;
    private CustomResponse res = new CustomResponse();
    private readonly ICommonService commonService;
    private BranchDTO dto = new BranchDTO();

    public BranchService(
        IUowProvider _uowProvider,
        IDapperContext _dapperContext,
        ICommonService _commonService
    )
    {
        uowProvider = _uowProvider;
        dapperContext = _dapperContext;
        commonService = _commonService;
    }
    
    private BranchValidation validation = new BranchValidation();


    public async Task<CustomResponse> PageInit(JObject obj)
    {
        string type = "PageInit";
        int branchId = obj["BranchId"].ToObject<int>();
        int companyId = obj["CompanyId"].ToObject<int>();

        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_BRANCH", new
                {
                    type,
                    branchId,
                    companyId
                });
                dto.UsersList = (await spcall.ReadAsync<User>()).ToList();
                dto.StatusList = (await spcall.ReadAsync<SystemMetadatum>()).ToList();
                if (branchId > 0)
                {
                    dto.branch = (await spcall.ReadAsync<Models.Branch>()).SingleOrDefault();
                }
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
        string type = "ListInit";
        int companyId = obj["CompanyId"].ToObject<int>();


        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_BRANCH", new
                {
                    type,
                    companyId
                });
                
                dto.BranchListInitList = (await spcall.ReadAsync<BranchListInit>()).ToList();
                
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

    public async Task<CustomResponse> Create(JObject obj)
    {
       Models.Branch branch = obj["Branch"].ToObject<Models.Branch>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var returnValidation = validation.Validations(branch);
                if (returnValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = returnValidation.Message;
                    res.Data = dto;
                    return res;
                }

                var branchRepo = uow.GetRepository<Models.Branch>();

                var duplicateValidation = validation.ValidateDuplicateForCreate(branch, branchRepo);
                if (duplicateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = duplicateValidation.Title;
                    res.Message = duplicateValidation.Message;
                    res.Data = dto;
                    return res;
                }

                branch.CreatedDate = DateTime.Now;
                branch.BranchCode = await commonService.GenerateUniqueCode(12);
                branchRepo.Add(branch);
                await uow.SaveChangesAsync();
            }

            res.IsSuccess = true;
            res.Title = "Success Message";
            res.Message = "Branch created successfully";
            res.Data = dto;
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
        Models.Branch branch = obj["Branch"].ToObject<Models.Branch>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var returnValidation = validation.Validations(branch);
                if (returnValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = returnValidation.Message;
                    res.Data = dto;
                    return res;
                }

                var branchRepo = uow.GetRepository<Models.Branch>();
                var userRepo = uow.GetRepository<User>();

                var duplicateValidation = validation.ValidateDuplicateForUpdate(branch, branchRepo);
                if (duplicateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = duplicateValidation.Title;
                    res.Message = duplicateValidation.Message;
                    res.Data = dto;
                    return res;
                }

                // Check if status is being changed to inactive
                if (branch.Status == 2)
                {
                    var canInactivateValidation = validation.ValidateCanInactivate(branch, userRepo);
                    if (canInactivateValidation.IsSuccess == false)
                    {
                        res.IsSuccess = false;
                        res.Title = canInactivateValidation.Title;
                        res.Message = canInactivateValidation.Message;
                        res.Data = dto;
                        return res;
                    }
                }

                branch.ModifiedDate = DateTime.Now;
                branchRepo.Update(branch);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Success Message";
                res.Message = "Branch details updated successfully";
                res.Data = dto;
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
        int BranchId = obj["BranchId"].ToObject<int>();
        int UserId = obj["UserId"].ToObject<int>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var branchRepo = uow.GetRepository<Models.Branch>();
                var userRepo = uow.GetRepository<User>();
                var branch = (await branchRepo.QueryAsync(x => x.BranchId == BranchId)).SingleOrDefault();

                var canInactivateValidation = validation.ValidateCanInactivate(branch, userRepo);
                if (canInactivateValidation.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.Title = canInactivateValidation.Title;
                    res.Message = canInactivateValidation.Message;
                    res.Data = dto;
                    return res;
                }

                branch.Status = 2;
                branch.ModifiedBy = UserId;
                branch.ModifiedDate = DateTime.Now;
                branchRepo.Update(branch);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Status Changed";
                res.Message = "Branch has become Inactive";
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