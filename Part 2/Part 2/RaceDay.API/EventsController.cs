
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        // Public event browsing.
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/Events/5
        // Public event details.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            return Ok(eventItem);
        }

        // POST: api/Events
        // Organiser only.
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event eventItem)
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
                    message = "Only Organisers can create events."
                });
            }

            if (string.IsNullOrWhiteSpace(eventItem.EventName))
            {
                return BadRequest(new
                {
                    message = "Event name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(eventItem.Location))
            {
                return BadRequest(new
                {
                    message = "Event location is required."
                });
            }

            if (eventItem.EventDate == default)
            {
                return BadRequest(new
                {
                    message = "A valid event date is required."
                });
            }

            // Ownership and creation time are assigned by the server.
            eventItem.OrganiserID = userID.Value;
            eventItem.CreatedAt = DateTime.UtcNow;

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventID },
                eventItem);
        }

        // PUT: api/Events/5
        // Organiser can update only their own events.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            Event updatedEvent)
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
                    message = "Only Organisers can update events."
                });
            }

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (existingEvent == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            if (existingEvent.OrganiserID != userID.Value)
            {
                return StatusCode(403, new
                {
                    message = "You can only update your own events."
                });
            }

            if (string.IsNullOrWhiteSpace(updatedEvent.EventName))
            {
                return BadRequest(new
                {
                    message = "Event name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(updatedEvent.Location))
            {
                return BadRequest(new
                {
                    message = "Event location is required."
                });
            }

            if (updatedEvent.EventDate == default)
            {
                return BadRequest(new
                {
                    message = "A valid event date is required."
                });
            }

            existingEvent.EventName = updatedEvent.EventName.Trim();
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.Location = updatedEvent.Location.Trim();
            existingEvent.RegistrationDeadline = updatedEvent.RegistrationDeadline;
            existingEvent.Status = updatedEvent.Status;

            // Do not accept a new OrganiserID or CreatedAt from the request.

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        // DELETE: api/Events/5
        // Organiser can delete only their own events.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
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
                    message = "Only Organisers can delete events."
                });
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            if (eventItem.OrganiserID != userID.Value)
            {
                return StatusCode(403, new
                {
                    message = "You can only delete your own events."
                });
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event deleted successfully."
            });
        }
    }
}
