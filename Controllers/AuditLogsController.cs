using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.Interfaces.Services;

namespace WorkforcePortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams)
    {
        var logs = await _auditLogService.GetAuditLogsAsync(paginationParams);
        return Ok(logs);
    }
}
