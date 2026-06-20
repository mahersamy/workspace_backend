namespace WorkforcePortal.Application.Interfaces;

using Repositories;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }
    IAuditLogRepository AuditLogs { get; }

    Task<int> SaveChangesAsync();
}
