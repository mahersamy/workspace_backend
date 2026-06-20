using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Employee;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeDto>> GetEmployeesAsync(PaginationParams paginationParams);
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    Task DeleteEmployeeAsync(int id);
}
