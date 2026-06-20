using AutoMapper;
using WorkforcePortal.Application.DTOs.Department;
using WorkforcePortal.Application.DTOs.Employee;
using WorkforcePortal.Application.DTOs.Task;
using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Department
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count));
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Employee
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Task
        CreateMap<AppTask, TaskDto>()
            .ForMember(dest => dest.AssignedToEmployeeName, opt => opt.MapFrom(src => src.AssignedToEmployee != null ? $"{src.AssignedToEmployee.FirstName} {src.AssignedToEmployee.LastName}" : null))
            .ForMember(dest => dest.CreatedByUsername, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Username : string.Empty))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
        CreateMap<CreateTaskDto, AppTask>();
        CreateMap<UpdateTaskDto, AppTask>();
    }
}
