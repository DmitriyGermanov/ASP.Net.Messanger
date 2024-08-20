using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Repo
{
    public partial class UserRepository(UserServiceContext context, IHttpContextAccessor httpContextAccessor) : IUserRepository
    {
        private readonly UserServiceContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid UserAdd(string email, string password, RoleId roleID)
        {
            try
            {
                ValidateEmail(email);

                if (_context.Users.Any(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException("This email is already used.");

                CheckAdminRole(roleID);

                var user = CreateUser(email, password, roleID);

                _context.Users.Add(user);
                _context.SaveChanges();

                return user.Id;
            }
            catch
            {
                throw;
            }
        }

        public (Guid userId, RoleId roleId) UserCheck(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == email)
                           ?? throw new Exception("User not found.");
            var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
            var bpassword = SHA512.HashData(data);

            if (user.Password.SequenceEqual(bpassword))
                return (user.Id, user.RoleId);
            else
                throw new Exception("Wrong password.");
        }

        public Guid GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim == null 
                                  ? throw new UnauthorizedAccessException("User is not authenticated.") : Guid.Parse(userIdClaim.Value);
        }

        public void DeleteUserById(Guid userId)
        {
            var currentUser = GetCurrentUser();

            if (currentUser.Id == userId)
                throw new InvalidOperationException("Admin cannot delete themselves.");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId)
                                   ?? throw new Exception("User not found.");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return [.. _context.Users.Select(user => new UserModel { Email = user.Email, RoleName = user.Role.Name })];
        }

        public Role GetRoleById(RoleId roleId)
        {
            return _context.Roles.FirstOrDefault(role => role.RoleId == roleId)
                   ?? throw new Exception("Role not found.");
        }

        private User GetCurrentUser()
        {
            var userId = GetUserIdFromToken();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            return user ?? throw new Exception("Current user not found.");
        }

        private User CreateUser(string email, string password, RoleId roleID)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = HashPassword(password, out var salt),
                Salt = salt,
                RoleId = roleID
            };

            var role = _context.Roles.SingleOrDefault(r => r.RoleId == roleID)
                       ?? throw new InvalidOperationException("Role not found.");
            user.Role = role;

            return user;
        }

        private static void ValidateEmail(string email)
        {
            var emailRegex = MyRegex();
            if (!emailRegex.IsMatch(email))
            {
                throw new ArgumentException("Incorrect email address.");
            }
        }

        private void CheckAdminRole(RoleId roleID)
        {
            if (roleID == RoleId.Admin)
            {
                var count = _context.Users.Count(x => x.RoleId == RoleId.Admin);
                if (count > 0)
                {
                    throw new Exception("Only one admin can be set.");
                }
            }
        }

        private static byte[] HashPassword(string password, out byte[] salt)
        {
            salt = new byte[16];
            new Random().NextBytes(salt);
            var data = Encoding.ASCII.GetBytes(password).Concat(salt).ToArray();
            return SHA512.HashData(data);
        }

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex MyRegex();
    }
}