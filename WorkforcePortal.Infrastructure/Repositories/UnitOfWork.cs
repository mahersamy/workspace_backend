using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IEmployeeRepository Employees { get; private set; }
    public IDepartmentRepository Departments { get; private set; }
    public ITaskRepository Tasks { get; private set; }
    public IUserRepository Users { get; private set; }
    public IAuditLogRepository AuditLogs { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(context);
        Departments = new DepartmentRepository(context);
        Tasks = new TaskRepository(context);
        Users = new UserRepository(context);
        AuditLogs = new AuditLogRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
