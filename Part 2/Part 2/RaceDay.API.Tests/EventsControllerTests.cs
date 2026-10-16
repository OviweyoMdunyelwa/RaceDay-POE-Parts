using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Tests
{
    public class EventsControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        private EventsController CreateController(
            RaceDayDbContext context,
            int? userID = null,
            string? role = null)
        {
            var controller = new EventsController(context);

            var httpContext = new DefaultHttpContext();

            httpContext.Session = new TestSession();

            if (userID.HasValue)
            {
                httpContext.Session.SetInt32(
                    "UserID",
                    userID.Value);
            }

            if (role != null)
            {
                httpContext.Session.SetString(
                    "Role",
                    role);
            }

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        [Fact]
        public void EventsController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new EventsController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task EventsDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var events = await context.Events.ToListAsync();

            Assert.Empty(events);
        }

        [Fact]
        public async Task GetEvents_ReturnsOk()
        {
            using var context = CreateDbContext();

            context.Events.Add(new Event
            {
                EventName = "Test Running Event",
                Description = "Test event",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            });

            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                1,
                "Participant");

            var result = await controller.GetEvents();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var events = Assert.IsAssignableFrom<IEnumerable<Event>>(
                okResult.Value);

            Assert.Single(events);
        }

        [Fact]
        public async Task GetEvent_WithValidId_ReturnsOk()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Test Event",
                Description = "Test event",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                1,
                "Participant");

            var result = await controller.GetEvent(
                eventItem.EventID);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEvent_WithInvalidId_ReturnsNotFound()
        {
            using var context = CreateDbContext();

            var controller = CreateController(
                context,
                1,
                "Participant");

            var result = await controller.GetEvent(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var context = CreateDbContext();

            var controller = CreateController(context);

            var eventItem = new Event
            {
                EventName = "Unauthorised Event",
                Description = "Test event",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open"
            };

            var result = await controller.CreateEvent(eventItem);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_AsParticipant_ReturnsForbid()
        {
            using var context = CreateDbContext();

            var controller = CreateController(
                context,
                10,
                "Participant");

            var eventItem = new Event
            {
                EventName = "Participant Event",
                Description = "Test event",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open"
            };

            var result = await controller.CreateEvent(eventItem);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task CreateEvent_AsOrganiser_ReturnsCreated()
        {
            using var context = CreateDbContext();

            var controller = CreateController(
                context,
                1,
                "Organiser");

            var eventItem = new Event
            {
                EventName = "Organiser Event",
                Description = "Created by organiser",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open"
            };

            var result = await controller.CreateEvent(eventItem);

            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(
                "GetEvent",
                createdResult.ActionName);

            Assert.Single(context.Events);

            var savedEvent = await context.Events.FirstAsync();

            Assert.Equal(1, savedEvent.OrganiserID);
        }

        [Fact]
        public async Task UpdateEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Existing Event",
                Description = "Existing",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var updatedEvent = new Event
            {
                EventID = eventItem.EventID,
                EventName = "Updated Event"
            };

            var result = await controller.UpdateEvent(
                eventItem.EventID,
                updatedEvent);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_AsParticipant_ReturnsForbid()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Existing Event",
                Description = "Existing",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                2,
                "Participant");

            var updatedEvent = new Event
            {
                EventID = eventItem.EventID,
                EventName = "Updated Event",
                Description = "Updated",
                EventDate = DateTime.Now.AddDays(40),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(25),
                Status = "Open"
            };

            var result = await controller.UpdateEvent(
                eventItem.EventID,
                updatedEvent);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_AsDifferentOrganiser_ReturnsForbid()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Existing Event",
                Description = "Existing",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                2,
                "Organiser");

            var updatedEvent = new Event
            {
                EventID = eventItem.EventID,
                EventName = "Updated Event",
                Description = "Updated",
                EventDate = DateTime.Now.AddDays(40),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(25),
                Status = "Open"
            };

            var result = await controller.UpdateEvent(
                eventItem.EventID,
                updatedEvent);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_AsEventOwner_ReturnsOk()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Original Event",
                Description = "Original",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                1,
                "Organiser");

            var updatedEvent = new Event
            {
                EventID = eventItem.EventID,
                EventName = "Updated Event",
                Description = "Updated description",
                EventDate = DateTime.Now.AddDays(40),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(25),
                Status = "Open"
            };

            var result = await controller.UpdateEvent(
                eventItem.EventID,
                updatedEvent);

            Assert.IsType<OkObjectResult>(result);

            var savedEvent = await context.Events
                .FirstAsync(e => e.EventID == eventItem.EventID);

            Assert.Equal(
                "Updated Event",
                savedEvent.EventName);
        }

        [Fact]
        public async Task UpdateEvent_WithInvalidId_ReturnsNotFound()
        {
            using var context = CreateDbContext();

            var controller = CreateController(
                context,
                1,
                "Organiser");

            var updatedEvent = new Event
            {
                EventID = 999,
                EventName = "Updated Event"
            };

            var result = await controller.UpdateEvent(
                999,
                updatedEvent);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Delete Event",
                Description = "Test",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.DeleteEvent(
                eventItem.EventID);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_AsParticipant_ReturnsForbid()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Delete Event",
                Description = "Test",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                2,
                "Participant");

            var result = await controller.DeleteEvent(
                eventItem.EventID);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_AsDifferentOrganiser_ReturnsForbid()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Delete Event",
                Description = "Test",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                2,
                "Organiser");

            var result = await controller.DeleteEvent(
                eventItem.EventID);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_AsEventOwner_ReturnsOk()
        {
            using var context = CreateDbContext();

            var eventItem = new Event
            {
                EventName = "Delete Event",
                Description = "Test",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Gqeberha",
                RegistrationDeadline = DateTime.Now.AddDays(20),
                Status = "Open",
                OrganiserID = 1,
                CreatedAt = DateTime.Now
            };

            context.Events.Add(eventItem);
            await context.SaveChangesAsync();

            var controller = CreateController(
                context,
                1,
                "Organiser");

            var result = await controller.DeleteEvent(
                eventItem.EventID);

            Assert.IsType<OkObjectResult>(result);

            Assert.Empty(context.Events);
        }

        [Fact]
        public async Task DeleteEvent_WithInvalidId_ReturnsNotFound()
        {
            using var context = CreateDbContext();

            var controller = CreateController(
                context,
                1,
                "Organiser");

            var result = await controller.DeleteEvent(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}