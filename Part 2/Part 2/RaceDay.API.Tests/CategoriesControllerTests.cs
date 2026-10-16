using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;

namespace RaceDay.API.Tests
{
    public class CategoriesControllerTests
    {
        private RaceDayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public void CategoriesController_CanBeCreated()
        {
            using var context = CreateDbContext();

            var controller = new CategoriesController(context);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task CategoriesDatabase_StartsEmpty()
        {
            using var context = CreateDbContext();

            var categories = await context.Categories.ToListAsync();

            Assert.Empty(categories);
        }
    }
}