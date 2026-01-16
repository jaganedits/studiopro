using Service.Models;
using Service.UnitOfWork.Repositories;
using Service.Utils;

namespace Service.Validation.Admin;

public class BranchValidation
{
    public CustomResponse Validations(Branch branch)
    {
        CustomResponse res = new CustomResponse();

        if (string.IsNullOrWhiteSpace(branch.BranchName))
        {
            res.IsSuccess = false;
            res.Title = "Warning message";
            res.Message = $"Branch Name is Required.";
            return res;
        }

        if (branch.Status <= 0)
        {
            res.IsSuccess = false;
            res.Title = "Warning message";
            res.Message = $"Status is Required.";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateDuplicateForCreate(Branch branch, IRepository<Branch> branchRepo)
    {
        CustomResponse res = new CustomResponse();

        if (branchRepo.Any(x =>
                x.BranchName.Trim().ToLower() == branch.BranchName.Trim().ToLower() &&
                x.CompanyId == branch.CompanyId && x.Status == 1))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Branch Name: {branch.BranchName} Already Exists.";
            return res;
        }

        if (branchRepo.Any(x =>
                x.BranchName.Trim().ToLower() == branch.BranchName.Trim().ToLower() &&
                x.CompanyId == branch.CompanyId && x.Status == 2))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Branch Name: {branch.BranchName} Already Exists in Inactive status. Please Active it";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateDuplicateForUpdate(Branch branch, IRepository<Branch> branchRepo)
    {
        CustomResponse res = new CustomResponse();

        if (branchRepo.Any(x =>
                x.BranchName.Trim().ToLower() == branch.BranchName.Trim().ToLower() &&
                x.CompanyId == branch.CompanyId && x.Status == 1 && x.BranchId != branch.BranchId))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Branch Name: {branch.BranchName} Already Exists";
            return res;
        }

        if (branchRepo.Any(x =>
                x.BranchName.Trim().ToLower() == branch.BranchName.Trim().ToLower() &&
                x.CompanyId == branch.CompanyId && x.Status == 2 && x.BranchId != branch.BranchId))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Branch Name: {branch.BranchName} Already Exists in Inactive status. Please Active it";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateCanInactivate(Branch branch, IRepository<User> userRepo)
    {
        CustomResponse res = new CustomResponse();

        if (userRepo.Any(x => x.BranchId == branch.BranchId && x.Status == 1))
        {
            res.IsSuccess = false;
            res.Title = "Cannot Inactivate";
            res.Message = $"Active users are mapped to {branch.BranchName}, so it cannot be inactivated.";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }
}