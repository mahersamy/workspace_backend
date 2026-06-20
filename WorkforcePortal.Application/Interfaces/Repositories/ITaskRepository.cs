using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Interfaces.Repositories;

public interface ITaskRepository : IGenericRepository<AppTask>
{
    Task<IEnumerable<AppTask>> GetTasksForEmployeeAsync(int employeeId);
    Task<AppTask?> GetTaskWithDetailsAsync(int id);
}
