using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.AuthorizationModel
{
    public class UserAuthenticationService(UserServiceContext context) : IUserAuthenticationService
    {
        private readonly UserServiceContext _context = context;

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

            if (user == null || user.Salt == null)
            {
                return null;
            }

            var data = Encoding.ASCII.GetBytes(model.Password).Concat(user.Salt).ToArray();
            var bpassword = SHA512.HashData(data);


            if (user.Password.SequenceEqual(bpassword))
            {
                return user;
            }
            else
            {
                throw new Exception("Wrong Password");
            }
        }
    }
}