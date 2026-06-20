using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Services;
using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _uow;

    public AuditLogService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<AuditLog>> GetAuditLogsAsync(PaginationParams paginationParams)
    {
        var logs = await _uow.AuditLogs.GetAllAsync();
        logs = logs.OrderByDescending(l => l.Timestamp);

        var total = logs.Count();
        var paged = logs.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                        .Take(paginationParams.PageSize);

        return new PagedResult<AuditLog>
        {
            Data = paged,
            TotalCount = total,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }
}
