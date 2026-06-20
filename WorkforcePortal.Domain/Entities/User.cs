using WorkforcePortal.Domain.Enums;

namespace WorkforcePortal.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Employee;
    public bool IsActive { get; set; } = true;

    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public ICollection<AppTask> CreatedTasks { get; set; } = new List<AppTask>();
}
