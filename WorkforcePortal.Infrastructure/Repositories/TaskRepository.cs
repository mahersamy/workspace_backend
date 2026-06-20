using Microsoft.EntityFrameworkCore;
using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class TaskRepository : GenericRepository<AppTask>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AppTask>> GetTasksForEmployeeAsync(int employeeId)
    {
        return await _dbSet.Include(t => t.AssignedToEmployee)
                           .Include(t => t.CreatedByUser)
                           .Where(t => t.AssignedToEmployeeId == employeeId)
                           .ToListAsync();
    }

    public async Task<AppTask?> GetTaskWithDetailsAsync(int id)
    {
        return await _dbSet.Include(t => t.AssignedToEmployee)
                           .Include(t => t.CreatedByUser)
                           .FirstOrDefaultAsync(t => t.Id == id);
    }
}
