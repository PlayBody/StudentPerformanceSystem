using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Data;
using StudentPerformanceSystem.Models;

namespace StudentPerformanceSystem.Services
{
    public class AuthService
    {
        private readonly StudentPerformanceContext _context;

        public AuthService(StudentPerformanceContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(string email, string password, string role, int? teacherId, int? schoolId)
        {
            if(_context.Users == null)
            {
                return false;
            }
            if (_context.Users.Any(u => u.Email == email))
                return false; // User already exists

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                TeacherID = role == "Teacher" ? teacherId : null,
                SchoolID = role == "SchoolLeader" ? schoolId : null
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            if (_context.Users == null)
            {
                return null;
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }
    }
}