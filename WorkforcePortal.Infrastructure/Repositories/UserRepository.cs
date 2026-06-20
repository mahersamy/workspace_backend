using Microsoft.EntityFrameworkCore;
using WorkforcePortal.Application.Interfaces.Repositories;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Infrastructure.Data;

namespace WorkforcePortal.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}
