using Microsoft.EntityFrameworkCore;
using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithDepartmentsAsync()
    {
        return await _dbSet.Include(e => e.Department).Where(e => e.IsActive).ToListAsync();
    }

    public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
    {
        return await _dbSet.Include(e => e.Department)
                           .Include(e => e.Tasks)
                           .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
    }
}
