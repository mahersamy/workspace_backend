using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Interfaces.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<IEnumerable<Employee>> GetEmployeesWithDepartmentsAsync();
    Task<Employee?> GetEmployeeWithDetailsAsync(int id);
}
