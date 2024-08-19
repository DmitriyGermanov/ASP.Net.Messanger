using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserService.AuthorizationModel;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserServiceTests
{
    [TestClass]
    public class UserAuthenticationServiceTests
    {
        private UserServiceContext? _context;
        private IUserAuthenticationService? _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDatabase")
                .Options;

            _context = new UserServiceContext(options);
            _service = new UserAuthenticationService(_context);

            var role = _context.Roles.FirstOrDefault(r => r.RoleId == RoleId.Admin);

            if (role == null)
            {
                role = new Role
                {
                    RoleId = RoleId.Admin,
                    Name = "Admin"
                };
                _context.Roles.Add(role);
                _context.SaveChanges();
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Salt = new byte[16],
                RoleId = role.RoleId, 
                Role = role 
            };

            var data = Encoding.ASCII.GetBytes("password123").Concat(user.Salt).ToArray();
            user.Password = SHA512.HashData(data);

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsUser()
        {
            var loginModel = new UserLoginModel
            {
                Email = "test@example.com",
                Password = "password123"
            };
            
            var result = _service?.Authenticate(loginModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(loginModel.Email, result.Email);
        }

        [TestMethod]
        public void Authenticate_InvalidPassword_ReturnsException()
        {
            var loginModel = new UserLoginModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            Assert.ThrowsException<Exception>(() => _service?.Authenticate(loginModel));
        }

        [TestMethod]
        public void Authenticate_InvalidEmail_ReturnsNull()
        {
            var loginModel = new UserLoginModel
            {
                Email = "nonexistent@example.com",
                Password = "password"
            };

            var result = _service?.Authenticate(loginModel);

            Assert.IsNull(result);
        }
    }
}