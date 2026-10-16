using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;

namespace RaceDay.API.Tests
{
    public class EnrolmentsControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public void EnrolmentsController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new EnrolmentsController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task EnrolmentsDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var enrolments = await context.Enrolments.ToListAsync();

            Assert.Empty(enrolments);
        }
    }
}