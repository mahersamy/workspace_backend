using AutoMapper;
using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Employee;
using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Services;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Domain.Exceptions;

namespace WorkforcePortal.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<EmployeeDto>> GetEmployeesAsync(PaginationParams paginationParams)
    {
        var employees = await _uow.Employees.GetEmployeesWithDepartmentsAsync();
        
        if (!string.IsNullOrEmpty(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.ToLower();
            employees = employees.Where(e => e.FirstName.ToLower().Contains(search) || 
                                             e.LastName.ToLower().Contains(search) ||
                                             e.Email.ToLower().Contains(search));
        }

        var total = employees.Count();
        var paged = employees.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                             .Take(paginationParams.PageSize);

        return new PagedResult<EmployeeDto>
        {
            Data = _mapper.Map<IEnumerable<EmployeeDto>>(paged),
            TotalCount = total,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var employee = await _uow.Employees.GetEmployeeWithDetailsAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var dept = await _uow.Departments.GetByIdAsync(dto.DepartmentId);
        if (dept == null)
            throw new NotFoundException(nameof(Department), dto.DepartmentId);

        var employee = _mapper.Map<Employee>(dto);
        await _uow.Employees.AddAsync(employee);
        await _uow.SaveChangesAsync();

        employee.Department = dept;
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _uow.Employees.GetByIdAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        if (employee.DepartmentId != dto.DepartmentId)
        {
            var dept = await _uow.Departments.GetByIdAsync(dto.DepartmentId);
            if (dept == null)
                throw new NotFoundException(nameof(Department), dto.DepartmentId);
        }

        _mapper.Map(dto, employee);
        _uow.Employees.Update(employee);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _uow.Employees.GetByIdAsync(id);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), id);

        employee.IsActive = false; // Soft delete
        _uow.Employees.Update(employee);
        await _uow.SaveChangesAsync();
    }
}
