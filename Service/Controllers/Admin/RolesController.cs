using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Service.Services.Admin.Role;

namespace Service.Controllers.Admin;

// [Authorize]
[ApiController]
[Route("[Controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolesService _iRolesService;

    public RolesController(IRolesService rolesService)
    {
        _iRolesService = rolesService;
    }

    [HttpPost("PageInit")]
    public async Task<IActionResult> PageInit(JObject obj)
    {
        try
        {
            return Ok(await _iRolesService.PageInit(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("ListInit")]
    public async Task<IActionResult> ListInit(JObject obj)
    {
        try
        {
            return Ok(await _iRolesService.ListInit(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(JObject obj)
    {
        try
        {
            return Ok(await _iRolesService.Create(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> Update(JObject obj)
    {
        try
        {
            return Ok(await _iRolesService.Update(obj));
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
            return Ok(await _iRolesService.ChangeStatus(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}