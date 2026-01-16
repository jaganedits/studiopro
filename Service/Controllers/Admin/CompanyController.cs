using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Services.Admin.Company;

namespace Service.Controllers.Admin;

[ApiController]
[Route("[Controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost("PageInit")]
    public async Task<IActionResult> PageInit(JObject obj)
    {
        try
        {
            return Ok(await _companyService.PageInit(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromForm] string data, IFormFile? logo, IFormFile? signature)
    {
        try
        {
            var obj = JObject.Parse(data);
            return Ok(await _companyService.Update(obj, logo, signature));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("CreateAddress")]
    public async Task<IActionResult> CreateAddress(JObject obj)
    {
        try
        {
            return Ok(await _companyService.CreateAddress(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("UpdateAddress")]
    public async Task<IActionResult> UpdateAddress(JObject obj)
    {
        try
        {
            return Ok(await _companyService.UpdateAddress(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("ChangeStatus")]
    public async Task<IActionResult> ChangeStatus(JObject obj)
    {
        try
        {
            return Ok(await _companyService.ChangeStatus(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
