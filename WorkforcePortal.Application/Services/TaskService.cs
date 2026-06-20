using AutoMapper;
using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Task;
using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Services;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Domain.Enums;
using WorkforcePortal.Domain.Exceptions;

namespace WorkforcePortal.Application.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TaskService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<TaskDto>> GetTasksAsync(PaginationParams paginationParams)
    {
        var tasks = await _uow.Tasks.GetAllAsync(); // Simplified; ideally eagerly load properties.
        return PaginateTasks(tasks, paginationParams);
    }

    public async Task<PagedResult<TaskDto>> GetTasksForEmployeeAsync(int employeeId, PaginationParams paginationParams)
    {
        var tasks = await _uow.Tasks.GetTasksForEmployeeAsync(employeeId);
        return PaginateTasks(tasks, paginationParams);
    }

    private PagedResult<TaskDto> PaginateTasks(IEnumerable<AppTask> tasks, PaginationParams paginationParams)
    {
        if (!string.IsNullOrEmpty(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.ToLower();
            tasks = tasks.Where(t => t.Title.ToLower().Contains(search) || 
                                     (t.Description != null && t.Description.ToLower().Contains(search)));
        }

        var total = tasks.Count();
        var paged = tasks.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                         .Take(paginationParams.PageSize);

        return new PagedResult<TaskDto>
        {
            Data = _mapper.Map<IEnumerable<TaskDto>>(paged),
            TotalCount = total,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }

    public async Task<TaskDto> GetTaskByIdAsync(int id)
    {
        var task = await _uow.Tasks.GetTaskWithDetailsAsync(id);
        if (task == null)
            throw new NotFoundException("Task", id);

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, int currentUserId)
    {
        if (dto.AssignedToEmployeeId.HasValue)
        {
            var emp = await _uow.Employees.GetByIdAsync(dto.AssignedToEmployeeId.Value);
            if (emp == null) throw new NotFoundException("Employee", dto.AssignedToEmployeeId.Value);
        }

        var task = _mapper.Map<AppTask>(dto);
        task.CreatedByUserId = currentUserId;

        await _uow.Tasks.AddAsync(task);
        await _uow.SaveChangesAsync();

        return await GetTaskByIdAsync(task.Id); // Reload to map correctly
    }

    public async Task UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task", id);

        if (dto.AssignedToEmployeeId.HasValue && dto.AssignedToEmployeeId != task.AssignedToEmployeeId)
        {
            var emp = await _uow.Employees.GetByIdAsync(dto.AssignedToEmployeeId.Value);
            if (emp == null) throw new NotFoundException("Employee", dto.AssignedToEmployeeId.Value);
        }

        _mapper.Map(dto, task);
        _uow.Tasks.Update(task);
        await _uow.SaveChangesAsync();
    }

    public async Task AssignTaskAsync(int id, AssignTaskDto dto)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task", id);

        var emp = await _uow.Employees.GetByIdAsync(dto.EmployeeId);
        if (emp == null) throw new NotFoundException("Employee", dto.EmployeeId);

        task.AssignedToEmployeeId = dto.EmployeeId;
        _uow.Tasks.Update(task);
        await _uow.SaveChangesAsync();
    }

    public async Task UpdateTaskStatusAsync(int id, WorkforcePortal.Domain.Enums.TaskStatus status, int currentUserId)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task", id);

        task.Status = status;
        _uow.Tasks.Update(task);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task", id);

        _uow.Tasks.Remove(task);
        await _uow.SaveChangesAsync();
    }
}
