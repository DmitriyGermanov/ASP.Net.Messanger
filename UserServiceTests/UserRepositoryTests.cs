using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.Models;
using UserService.Repo;

namespace UserServiceTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private UserServiceContext? _context;
        private UserRepository? _repository;
        private Mock<IHttpContextAccessor>? _httpContextAccessorMock;
        private DbContextOptions<UserServiceContext>? _dbContextOptions;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDatabase")
                .Options;

            _context = new UserServiceContext(_dbContextOptions);
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _repository = new UserRepository(_context, _httpContextAccessorMock.Object);
            var adminRole = _context.Roles.FirstOrDefault(r => r.RoleId == RoleId.Admin);
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    RoleId = RoleId.Admin,
                    Name = "Admin"
                };
                _context.Roles.Add(adminRole);
                _context.SaveChanges();
            }

            var userRole = _context.Roles.FirstOrDefault(r => r.RoleId == RoleId.User);
            if (userRole == null)
            {
                userRole = new Role
                {
                    RoleId = RoleId.User,
                    Name = "Admin"
                };
                _context.Roles.Add(userRole);
                _context.SaveChanges();
            }

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.com",
                Password = HashPassword("admin123", out var salt),
                Role = adminRole,
                Salt = salt
            };

            _context.Users.Add(adminUser);
            _context.SaveChanges();
        }

        [TestMethod]
        public void UserAdd_ShouldAddUser()
        {
            var email = "user@test.com";
            var password = "password123";

            _repository?.UserAdd(email, password, RoleId.User);

            var user = _context?.Users.FirstOrDefault(u => u.Email == email);

            Assert.IsNotNull(user);
            Assert.AreEqual(email, user?.Email);
            Assert.AreEqual(RoleId.User, user?.RoleId);
        }

        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var users = _repository?.GetUsers();

            Assert.IsTrue(users?.Any());
        }


        [TestMethod]
        public void GetRoleById_ShouldReturnRole()
        {
            var roleId = RoleId.Admin;

            var role = _repository?.GetRoleById(roleId);

            Assert.IsNotNull(role);
            Assert.AreEqual(roleId, role.RoleId);
        }

        private static byte[] HashPassword(string password, out byte[] salt)
        {
            salt = new byte[16];
            new Random().NextBytes(salt);
            var data = Encoding.ASCII.GetBytes(password).Concat(salt).ToArray();
            return SHA512.HashData(data);
        }

    }

}