using WorkforcePortal.Application.DTOs.Common;
using WorkforcePortal.Application.DTOs.Task;
using WorkforcePortal.Domain.Enums;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface ITaskService
{
    Task<PagedResult<TaskDto>> GetTasksAsync(PaginationParams paginationParams);
    Task<PagedResult<TaskDto>> GetTasksForEmployeeAsync(int employeeId, PaginationParams paginationParams);
    Task<TaskDto> GetTaskByIdAsync(int id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, int currentUserId);
    Task UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task AssignTaskAsync(int id, AssignTaskDto dto);
    Task UpdateTaskStatusAsync(int id, WorkforcePortal.Domain.Enums.TaskStatus status, int currentUserId);
    Task DeleteTaskAsync(int id);
}
