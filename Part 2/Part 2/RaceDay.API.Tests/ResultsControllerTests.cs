using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;

namespace RaceDay.API.Tests
{
    public class ResultsControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public void ResultsController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new ResultsController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task ResultsDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var results = await context.Results.ToListAsync();

            Assert.Empty(results);
        }
    }
}