using FullStackApp.API.Models;
using FullStackApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FullStackApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        public UserService(AppDbContext db) { _db = db; }

        public async Task<User> RegisterAsync(string email, string password, UserRole role)
        {
            var user = new User { Email = email, PasswordHash = password, Role = role };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<bool> ChangeUserRoleAsync(int id, UserRole newRole)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;
            user.Role = newRole;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
