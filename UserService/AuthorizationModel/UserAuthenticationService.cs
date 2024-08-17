using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.AuthorizationModel
{
    public class UserAuthenticationService(UserServiceContext context, IPasswordHasher<User> passwordHasher) : IUserAuthenticationService
    {
        private readonly UserServiceContext _context = context;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public User? Authenticate(UserLoginModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return null;
            }

                var user = _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == model.Email);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}