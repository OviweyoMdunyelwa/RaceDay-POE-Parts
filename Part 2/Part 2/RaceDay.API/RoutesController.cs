using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public RoutesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Routes
        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            var routes = await _context.Routes.ToListAsync();

            return Ok(routes);
        }

        // GET: api/Routes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoute(int id)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteID == id);

            if (route == null)
            {
                return NotFound(new
                {
                    message = "Route not found."
                });
            }

            return Ok(route);
        }

        // POST: api/Routes
        [HttpPost]
        public async Task<IActionResult> CreateRoute(
            RaceDay.API.Models.Route route)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only Organisers can create routes."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == route.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            _context.Routes.Add(route);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRoute),
                new { id = route.RouteID },
                route);
        }

        // PUT: api/Routes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(
            int id,
            RaceDay.API.Models.Route updatedRoute)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only Organisers can update routes."
                });
            }

            var existingRoute = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteID == id);

            if (existingRoute == null)
            {
                return NotFound(new
                {
                    message = "Route not found."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == updatedRoute.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            existingRoute.EventID = updatedRoute.EventID;
            existingRoute.RouteName = updatedRoute.RouteName;
            existingRoute.DistanceKM = updatedRoute.DistanceKM;
            existingRoute.ElevationGain = updatedRoute.ElevationGain;
            existingRoute.Description = updatedRoute.Description;
            existingRoute.MapURL = updatedRoute.MapURL;

            await _context.SaveChangesAsync();

            return Ok(existingRoute);
        }

        // DELETE: api/Routes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only Organisers can delete routes."
                });
            }

            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteID == id);

            if (route == null)
            {
                return NotFound(new
                {
                    message = "Route not found."
                });
            }

            _context.Routes.Remove(route);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Route deleted successfully."
            });
        }
    }
}