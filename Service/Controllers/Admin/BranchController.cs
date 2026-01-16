using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Service.Services.Admin.Branch;

namespace Service.Controllers.Admin;

[ApiController]
[Route("[Controller]")]
public class BranchController :ControllerBase
{
    private readonly IBranchService _IBranchService;

    public BranchController(IBranchService branchService)
    {
        _IBranchService = branchService;
    }
    
    [HttpPost("PageInit")]
    public async Task<IActionResult> PageInit(JObject obj)
    {
        try
        {
            return Ok(await _IBranchService.PageInit(obj));
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
            return Ok(await _IBranchService.ListInit(obj));
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
            return Ok(await _IBranchService.Create(obj));
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
            return Ok(await _IBranchService.Update(obj));
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
            return Ok(await _IBranchService.ChangeStatus(obj));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}