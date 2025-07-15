using FullStackApp.API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FullStackApp.API.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string email, string password, UserRole role);
        Task<User> AuthenticateAsync(string email, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> ChangeUserRoleAsync(int id, UserRole newRole);
    }
}
