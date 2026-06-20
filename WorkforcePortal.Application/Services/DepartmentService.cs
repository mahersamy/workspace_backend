using AutoMapper;
using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Department;
using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Services;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Domain.Exceptions;

namespace WorkforcePortal.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(PaginationParams paginationParams)
    {
        var departments = await _uow.Departments.GetAllAsync();
        
        if (!string.IsNullOrEmpty(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.ToLower();
            departments = departments.Where(d => d.Name.ToLower().Contains(search) || 
                                                (d.Description != null && d.Description.ToLower().Contains(search)));
        }

        var total = departments.Count();
        var paged = departments.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                               .Take(paginationParams.PageSize);

        return new PagedResult<DepartmentDto>
        {
            Data = _mapper.Map<IEnumerable<DepartmentDto>>(paged),
            TotalCount = total,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var dept = await _uow.Departments.GetDepartmentWithEmployeesAsync(id);
        if (dept == null)
            throw new NotFoundException(nameof(Department), id);

        
        return _mapper.Map<DepartmentDto>(dept);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var dept = _mapper.Map<Department>(dto);
        await _uow.Departments.AddAsync(dept);
        await _uow.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(dept);
    }

    public async Task UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
    {
        var dept = await _uow.Departments.GetByIdAsync(id);
        if (dept == null)
            throw new NotFoundException(nameof(Department), id);

        _mapper.Map(dto, dept);
        _uow.Departments.Update(dept);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var dept = await _uow.Departments.GetDepartmentWithEmployeesAsync(id);
        if (dept == null)
            throw new NotFoundException(nameof(Department), id);

        if (dept.Employees.Any())
            throw new DomainException("Cannot delete a department that has employees.");

        _uow.Departments.Remove(dept);
        await _uow.SaveChangesAsync();
    }
}
