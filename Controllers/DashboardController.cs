using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalEmployees = await _context.Employees.CountAsync(e => e.IsActive);
        var totalDepartments = await _context.Departments.CountAsync();
        
        var tasksByStatus = await _context.Tasks
            .GroupBy(t => t.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        return Ok(new
        {
            TotalEmployees = totalEmployees,
            TotalDepartments = totalDepartments,
            TasksByStatus = tasksByStatus
        });
    }
}
