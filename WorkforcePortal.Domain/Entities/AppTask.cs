using WorkforcePortal.Domain.Enums;

namespace WorkforcePortal.Domain.Entities;

public class AppTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public WorkforcePortal.Domain.Enums.TaskStatus Status { get; set; } = WorkforcePortal.Domain.Enums.TaskStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? AssignedToEmployeeId { get; set; }
    public Employee? AssignedToEmployee { get; set; }

    public int CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
