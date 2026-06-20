using Microsoft.EntityFrameworkCore;
using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Department?> GetDepartmentWithEmployeesAsync(int id)
    {
        return await _dbSet.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
    }
}
