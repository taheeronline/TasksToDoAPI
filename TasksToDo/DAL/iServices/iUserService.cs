using TasksToDo.Models;

namespace TasksToDo.DAL.iServices
{
    public interface iUserService
    {
        Task<User> GetUserByEmailAsync(string email);

        Task AddUserAsync(User user);
    }
}
