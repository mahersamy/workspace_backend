using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext context) : base(context)
    {
    }
}
