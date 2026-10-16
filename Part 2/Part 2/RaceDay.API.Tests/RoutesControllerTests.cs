using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;

namespace RaceDay.API.Tests
{
    public class RoutesControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public void RoutesController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new RoutesController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task RoutesDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var routes = await context.Routes.ToListAsync();

            Assert.Empty(routes);
        }
    }
}