using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Domain.Enums;

namespace WorkforcePortal.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Departments.Any())
        {
            var hr = new Department { Name = "HR", Description = "Human Resources" };
            var it = new Department { Name = "IT", Description = "Information Technology" };
            var sales = new Department { Name = "Sales", Description = "Sales Department" };

            await context.Departments.AddRangeAsync(hr, it, sales);
            await context.SaveChangesAsync();

            if (!context.Employees.Any())
            {
                var emp1 = new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com", DepartmentId = it.Id, Salary = 80000, HireDate = DateTime.UtcNow.AddYears(-2) };
                var emp2 = new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", DepartmentId = hr.Id, Salary = 70000, HireDate = DateTime.UtcNow.AddYears(-1) };

                await context.Employees.AddRangeAsync(emp1, emp2);
                await context.SaveChangesAsync();

                if (!context.Users.Any())
                {
                    var admin = new User 
                    { 
                        Username = "admin", 
                        Email = "admin@example.com", 
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), 
                        Role = UserRole.Admin,
                        EmployeeId = emp1.Id
                    };
                    
                    await context.Users.AddAsync(admin);
                    await context.SaveChangesAsync();

                    if (!context.Tasks.Any())
                    {
                        var task1 = new AppTask 
                        { 
                            Title = "Setup Workstation", 
                            Description = "Setup new employee workstation", 
                            Priority = TaskPriority.High, 
                            Status = WorkforcePortal.Domain.Enums.TaskStatus.Pending, 
                            AssignedToEmployeeId = emp1.Id,
                            CreatedByUserId = admin.Id
                        };

                        await context.Tasks.AddAsync(task1);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
