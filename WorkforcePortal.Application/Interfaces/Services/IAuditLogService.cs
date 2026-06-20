using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Application.DTOs.Common;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task<PagedResult<AuditLog>> GetAuditLogsAsync(PaginationParams paginationParams);
}
