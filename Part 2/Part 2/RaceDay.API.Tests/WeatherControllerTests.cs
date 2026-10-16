using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;

namespace RaceDay.API.Tests
{
    public class WeatherControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public void WeatherController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new WeatherController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task WeatherDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var weather = await context.Weather.ToListAsync();

            Assert.Empty(weather);
        }
    }
}