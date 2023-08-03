using Microsoft.EntityFrameworkCore;
using TasksToDo.DAL.iServices;
using TasksToDo.Models;

namespace TasksToDo.DAL.Services
{
    public class UserService:iUserService
    {
        private readonly ApplicationDBContext _context;

        public UserService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>().SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
