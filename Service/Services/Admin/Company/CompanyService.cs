using Newtonsoft.Json.Linq;
using Service.ContextHelpers;
using Service.DTO.Users;
using Service.Helper.FileUploadService;
using Service.Models;
using Service.Services.Helper.CommonService;
using Service.UnitOfWork.Uow;
using Service.Utils;

namespace Service.Services.Admin.Company;

public class CompanyService : ICompanyService
{
    private readonly IUowProvider uowProvider;
    private readonly IDapperContext dapperContext;
    private CustomResponse res = new CustomResponse();
    private readonly ICommonService commonService;
    private readonly IFileUploadService _fileUploadService;
    private CompanyDTO dto = new CompanyDTO();

    public CompanyService(
        IUowProvider _uowProvider,
        IDapperContext _dapperContext,
        ICommonService _commonService,
        IFileUploadService fileUploadService
    )
    {
        uowProvider = _uowProvider;
        dapperContext = _dapperContext;
        commonService = _commonService;
        _fileUploadService = fileUploadService;
    }

    public async Task<CustomResponse> PageInit(JObject obj)
    {
        string type = "PageInit";
        int companyId = obj["CompanyId"].ToObject<int>();

        try
        {
            using (dapperContext)
            {
                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_COMPANY", new
                {
                    type,
                    companyId
                });
                dto.StatusList = (await spcall.ReadAsync<SystemMetadatum>()).ToList();
                if (companyId > 0)
                {
                    dto.Company = (await spcall.ReadAsync<Models.Company>()).SingleOrDefault();
                    dto.AddressList = (await spcall.ReadAsync<CompanyAddress>()).ToList();
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

    public async Task<CustomResponse> Update(JObject obj, IFormFile? logo, IFormFile? signature)
    {
        Models.Company company = obj["Company"].ToObject<Models.Company>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var companyRepo = uow.GetRepository<Models.Company>();
                var existingCompany = (await companyRepo.QueryAsync(x => x.CompanyId == company.CompanyId)).SingleOrDefault();

                if (existingCompany == null)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = "Company not found";
                    return res;
                }

                // Handle logo upload
                if (logo != null && logo.Length > 0)
                {
                    var logoPath = await _fileUploadService.UploadFileWithUrl(logo, $"Company/{company.CompanyId}/Logo");
                    existingCompany.LogoPath = logoPath;
                    existingCompany.LogonName = logo.FileName;
                }

                // Handle signature upload
                if (signature != null && signature.Length > 0)
                {
                    var signaturePath = await _fileUploadService.UploadFileWithUrl(signature, $"Company/{company.CompanyId}/Signature");
                    existingCompany.SignaturePath = signaturePath;
                    existingCompany.SignaturenName = signature.FileName;
                }

                // Update company details
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.Tagline = company.Tagline;
                existingCompany.Phone = company.Phone;
                existingCompany.AlternatePhone = company.AlternatePhone;
                existingCompany.Email = company.Email;
                existingCompany.Website = company.Website;
                existingCompany.Facebook = company.Facebook;
                existingCompany.Instagram = company.Instagram;
                existingCompany.Youtube = company.Youtube;
                existingCompany.Whatsapp = company.Whatsapp;
                existingCompany.GstNumber = company.GstNumber;
                existingCompany.PanNumber = company.PanNumber;
                existingCompany.CinNumber = company.CinNumber;
                existingCompany.BankName = company.BankName;
                existingCompany.AccountNumber = company.AccountNumber;
                existingCompany.AccountHolderName = company.AccountHolderName;
                existingCompany.IfscCode = company.IfscCode;
                existingCompany.BranchName = company.BranchName;
                existingCompany.UpiId = company.UpiId;
                existingCompany.InvoicePrefix = company.InvoicePrefix;
                existingCompany.InvoiceStartNumber = company.InvoiceStartNumber;
                existingCompany.TermsConditions = company.TermsConditions;
                existingCompany.FooterNote = company.FooterNote;
                existingCompany.ModifiedBy = company.ModifiedBy;
                existingCompany.ModifiedDate = DateTime.Now;

                companyRepo.Update(existingCompany);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Success Message";
                res.Message = "Company details updated successfully";
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

    public async Task<CustomResponse> CreateAddress(JObject obj)
    {
        CompanyAddress address = obj["Address"].ToObject<CompanyAddress>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var addressRepo = uow.GetRepository<CompanyAddress>();

                // If this is set as primary, unset other primary addresses
                if (address.IsPrimary)
                {
                    var existingAddresses = await addressRepo.QueryAsync(x => x.CompanyId == address.CompanyId && x.IsPrimary);
                    foreach (var existingAddress in existingAddresses)
                    {
                        existingAddress.IsPrimary = false;
                        addressRepo.Update(existingAddress);
                    }
                }

                address.Status = 1;
                address.CreatedDate = DateTime.Now;
                addressRepo.Add(address);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Success Message";
                res.Message = "Address created successfully";
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

    public async Task<CustomResponse> UpdateAddress(JObject obj)
    {
        CompanyAddress address = obj["Address"].ToObject<CompanyAddress>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var addressRepo = uow.GetRepository<CompanyAddress>();
                var existingAddress = (await addressRepo.QueryAsync(x => x.CompanyAddressId == address.CompanyAddressId)).SingleOrDefault();

                if (existingAddress == null)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = "Address not found";
                    return res;
                }

                // If this is set as primary, unset other primary addresses
                if (address.IsPrimary && !existingAddress.IsPrimary)
                {
                    var otherAddresses = await addressRepo.QueryAsync(x => x.CompanyId == address.CompanyId && x.IsPrimary && x.CompanyAddressId != address.CompanyAddressId);
                    foreach (var otherAddress in otherAddresses)
                    {
                        otherAddress.IsPrimary = false;
                        addressRepo.Update(otherAddress);
                    }
                }

                existingAddress.Label = address.Label;
                existingAddress.Address = address.Address;
                existingAddress.Area = address.Area;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.Pincode = address.Pincode;
                existingAddress.Landmark = address.Landmark;
                existingAddress.IsPrimary = address.IsPrimary;
                existingAddress.ModifiedBy = address.ModifiedBy;
                existingAddress.ModifiedDate = DateTime.Now;

                addressRepo.Update(existingAddress);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Success Message";
                res.Message = "Address updated successfully";
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
        int companyAddressId = obj["CompanyAddressId"].ToObject<int>();
        int userId = obj["UserId"].ToObject<int>();
        try
        {
            using (var uow = uowProvider.CreateUnitOfWork())
            {
                var companyRepo = uow.GetRepository<Models.CompanyAddress>();
                var company = (await companyRepo.QueryAsync(x => x.CompanyAddressId == companyAddressId)).SingleOrDefault();

                if (company == null)
                {
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = "Company not found";
                    return res;
                }

                company.Status = 2; // Inactive
                company.ModifiedBy = userId;
                company.ModifiedDate = DateTime.Now;
                companyRepo.Update(company);
                await uow.SaveChangesAsync();

                res.IsSuccess = true;
                res.Title = "Status Changed";
                res.Message = "Company has become Inactive";
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