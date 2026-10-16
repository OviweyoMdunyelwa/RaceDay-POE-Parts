using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Tests
{
    public class AuthControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        private AuthController CreateController(RaceDayDbContext context)
        {
            var controller = new AuthController(context);

            var httpContext = new DefaultHttpContext();

            httpContext.Session = new TestSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        [Fact]
        public async Task Register_WithValidDetails_ReturnsCreated()
        {
            using var context = CreateDbContext();
            var controller = CreateController(context);

            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "test@example.com",
                Password = "Password123",
                Role = "Participant",
                PhoneNumber = "0123456789"
            };

            var result = await controller.Register(request);

            var createdResult = Assert.IsType<CreatedResult>(result);

            Assert.Equal(201, createdResult.StatusCode);
            Assert.Single(context.RaceDayUsers);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsConflict()
        {
            using var context = CreateDbContext();

            var existingUser = new RaceDayUser
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "duplicate@example.com",
                PasswordHash = "hashed-password",
                Role = "Participant",
                PhoneNumber = "0123456789",
                CreatedAt = DateTime.Now
            };

            context.RaceDayUsers.Add(existingUser);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var request = new RegisterRequest
            {
                FirstName = "New",
                LastName = "User",
                Email = "duplicate@example.com",
                Password = "Password123",
                Role = "Participant",
                PhoneNumber = "0123456789"
            };

            var result = await controller.Register(request);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithInvalidRole_ReturnsBadRequest()
        {
            using var context = CreateDbContext();
            var controller = CreateController(context);

            var request = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "invalidrole@example.com",
                Password = "Password123",
                Role = "Admin",
                PhoneNumber = "0123456789"
            };

            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
        {
            using var context = CreateDbContext();
            var controller = CreateController(context);

            var request = new LoginRequest
            {
                Email = "unknown@example.com",
                Password = "Password123"
            };

            var result = await controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ReturnsUnauthorized()
        {
            using var context = CreateDbContext();

            var registerController = CreateController(context);

            await registerController.Register(new RegisterRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "login@example.com",
                Password = "CorrectPassword123",
                Role = "Participant",
                PhoneNumber = "0123456789"
            });

            var loginController = CreateController(context);

            var request = new LoginRequest
            {
                Email = "login@example.com",
                Password = "WrongPassword123"
            };

            var result = await loginController.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithValidDetails_StoresUserIdAndRoleInSession()
        {
            using var context = CreateDbContext();

            var registerController = CreateController(context);

            await registerController.Register(new RegisterRequest
            {
                FirstName = "Test",
                LastName = "Organiser",
                Email = "session@example.com",
                Password = "Password123",
                Role = "Organiser",
                PhoneNumber = "0123456789"
            });

            var loginController = CreateController(context);

            var request = new LoginRequest
            {
                Email = "session@example.com",
                Password = "Password123"
            };

            var result = await loginController.Login(request);

            Assert.IsType<OkObjectResult>(result);

            var user = await context.RaceDayUsers
                .FirstAsync(u => u.Email == "session@example.com");

            var session = loginController.HttpContext.Session;

            Assert.Equal(
                user.UserID,
                session.GetInt32("UserID"));

            Assert.Equal(
                "Organiser",
                session.GetString("Role"));
        }
    }

    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;

        public string Id => "TestSession";

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }

        public bool TryGetValue(
            string key,
            out byte[]? value)
        {
            return _sessionStorage.TryGetValue(
                key,
                out value);
        }
    }
}