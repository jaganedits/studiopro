using Service.Models;
using Service.UnitOfWork.Repositories;
using Service.Utils;

namespace Service.Validation.Admin;

public class RoleValidation
{
    public CustomResponse Validations(Role role)
    {
        CustomResponse res = new CustomResponse();

        if (string.IsNullOrWhiteSpace(role.RoleName))
        {
            res.IsSuccess = false;
            res.Title = "Warning message";
            res.Message = $"Role Name is Required.";
            return res;
        }

        if (role.Status <= 0)
        {
            res.IsSuccess = false;
            res.Title = "Warning message";
            res.Message = $"Status is Required.";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateDuplicateForCreate(Role role, IRepository<Role> roleRepo)
    {
        CustomResponse res = new CustomResponse();

        if (roleRepo.Any(x =>
                x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower() &&
                x.CompanyId == role.CompanyId && x.Status == 1))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Role Name: {role.RoleName} Already Exists.";
            return res;
        }

        if (roleRepo.Any(x =>
                x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower() &&
                x.CompanyId == role.CompanyId && x.Status == 2))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Role Name: {role.RoleName} Already Exists in Inactive status. Please Active it";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateDuplicateForUpdate(Role role, IRepository<Role> roleRepo)
    {
        CustomResponse res = new CustomResponse();

        if (roleRepo.Any(x =>
                x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower() &&
                x.CompanyId == role.CompanyId && x.Status == 1 && x.RoleId != role.RoleId))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Role Name: {role.RoleName} Already Exists";
            return res;
        }

        if (roleRepo.Any(x =>
                x.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower() &&
                x.CompanyId == role.CompanyId && x.Status == 2 && x.RoleId != role.RoleId))
        {
            res.IsSuccess = false;
            res.Title = "Already Exists";
            res.Message = $"Role Name: {role.RoleName} Already Exists in Inactive status. Please Active it";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }

    public CustomResponse ValidateCanInactivate(Role role, IRepository<User> userRepo)
    {
        CustomResponse res = new CustomResponse();

        if (userRepo.Any(x => x.RoleId == role.RoleId && x.Status == 1))
        {
            res.IsSuccess = false;
            res.Title = "Cannot Inactivate";
            res.Message = $"Active users are mapped to {role.RoleName}, so it cannot be inactivated.";
            return res;
        }

        res.IsSuccess = true;
        return res;
    }
}