using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private UserAuthenticationService? _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDatabase")
                .Options;

            _context = new UserServiceContext(options);

            var passwordHasher = new PasswordHasher<User>();

            _service = new UserAuthenticationService(_context, passwordHasher);

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "User"
            };
            _context.Roles.Add(role);
            _context.SaveChanges();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                RoleId = role.Id 
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "password123");

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
        public void Authenticate_InvalidPassword_ReturnsNull()
        {
            var loginModel = new UserLoginModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var result = _service?.Authenticate(loginModel);

            Assert.IsNull(result);
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