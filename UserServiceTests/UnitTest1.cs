using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.AuthorizationModel;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserServiceTests
{
    [TestClass]
    public class UserAuthenticationServiceTests
    {
        private Mock<UserServiceContext>? _mockContext;
        private Mock<IPasswordHasher<User>>? _mockPasswordHasher;
        private UserAuthenticationService? _service;
        private List<User>? _users;

        [TestInitialize]
        public void Setup()
        {
            _users =
            [
                new User { Id = Guid.NewGuid(), Email = "test@test.com", PasswordHash = "hashedPassword", Role = new Role { Name = "User" } }
            ];

            // Create a mock DbSet<User>
            var mockUserDbSet = new Mock<DbSet<User>>();
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_users.AsQueryable().Provider);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_users.AsQueryable().Expression);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_users.AsQueryable().ElementType);
            mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_users.GetEnumerator());

            _mockContext = new Mock<UserServiceContext>("YourConnectionStringHere");
            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _service = new UserAuthenticationService(_mockContext.Object, _mockPasswordHasher.Object);
        }

        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsUser()
        {
            var loginModel = new UserLoginModel { Email = "test@test.com", Password = "password" };
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), "hashedPassword", "password"))
                               .Returns(PasswordVerificationResult.Success);

            var result = _service.Authenticate(loginModel);

            Assert.IsNotNull(result);
            Assert.AreEqual("test@test.com", result.Email);
        }

        [TestMethod]
        public void Authenticate_InvalidPassword_ReturnsNull()
        {
            var loginModel = new UserLoginModel { Email = "test@test.com", Password = "wrongPassword" };
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), "hashedPassword", "wrongPassword"))
                               .Returns(PasswordVerificationResult.Failed);

            var result = _service.Authenticate(loginModel);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Authenticate_NonExistentUser_ReturnsNull()
        {
            var loginModel = new UserLoginModel { Email = "nonexistent@test.com", Password = "password" };

            var result = _service.Authenticate(loginModel);

            Assert.IsNull(result);
        }
    }
}