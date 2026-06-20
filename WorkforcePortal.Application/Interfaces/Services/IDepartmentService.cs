using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Department;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(PaginationParams paginationParams);
    Task<DepartmentDto> GetDepartmentByIdAsync(int id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
    Task UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
    Task DeleteDepartmentAsync(int id);
}
