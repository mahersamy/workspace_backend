using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetDepartmentWithEmployeesAsync(int id);
}
