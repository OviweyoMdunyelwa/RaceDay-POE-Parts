
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Results
        // Organisers can view all results.
        // Participants can view only their own results.
        [HttpGet]
        public async Task<IActionResult> GetResults()
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

            if (role == "Organiser")
            {
                var allResults = await _context.Results.ToListAsync();
                return Ok(allResults);
            }

            if (role == "Participant")
            {
                var participantResults = await _context.Results
                    .Where(r => _context.Enrolments.Any(e =>
                        e.EnrolmentID == r.EnrolmentID &&
                        e.ParticipantID == userID.Value))
                    .ToListAsync();

                return Ok(participantResults);
            }

            return StatusCode(403, new
            {
                message = "You do not have permission to view results."
            });
        }

        // GET: api/Results/1
        // Organisers can view any result.
        // Participants can view only their own results.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResult(int id)
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

            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            if (role == "Participant")
            {
                bool belongsToParticipant = await _context.Enrolments
                    .AnyAsync(e =>
                        e.EnrolmentID == result.EnrolmentID &&
                        e.ParticipantID == userID.Value);

                if (!belongsToParticipant)
                {
                    return StatusCode(403, new
                    {
                        message = "You can only view your own results."
                    });
                }
            }
            else if (role != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "You do not have permission to view results."
                });
            }

            return Ok(result);
        }

        // POST: api/Results
        // Organiser only.
        [HttpPost]
        public async Task<IActionResult> CreateResult(Result result)
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
                    message = "Only Organisers can create results."
                });
            }

            bool enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == result.EnrolmentID);

            if (!enrolmentExists)
            {
                return BadRequest(new
                {
                    message = "The specified enrolment does not exist."
                });
            }

            bool resultExists = await _context.Results
                .AnyAsync(r => r.EnrolmentID == result.EnrolmentID);

            if (resultExists)
            {
                return Conflict(new
                {
                    message = "A result already exists for this enrolment."
                });
            }

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }

        // PUT: api/Results/1
        // Organiser only.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(
            int id,
            Result updatedResult)
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
                    message = "Only Organisers can update results."
                });
            }

            var existingResult = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (existingResult == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            bool enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == updatedResult.EnrolmentID);

            if (!enrolmentExists)
            {
                return BadRequest(new
                {
                    message = "The specified enrolment does not exist."
                });
            }

            bool duplicateResult = await _context.Results
                .AnyAsync(r =>
                    r.EnrolmentID == updatedResult.EnrolmentID &&
                    r.ResultID != id);

            if (duplicateResult)
            {
                return Conflict(new
                {
                    message = "Another result already exists for this enrolment."
                });
            }

            existingResult.EnrolmentID = updatedResult.EnrolmentID;
            existingResult.FinishTime = updatedResult.FinishTime;
            existingResult.ChipTime = updatedResult.ChipTime;
            existingResult.PositionOverall = updatedResult.PositionOverall;
            existingResult.PositionCategory = updatedResult.PositionCategory;
            existingResult.Pace = updatedResult.Pace;
            existingResult.Status = updatedResult.Status;

            await _context.SaveChangesAsync();

            return Ok(existingResult);
        }

        // DELETE: api/Results/1
        // Organiser only.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
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
                    message = "Only Organisers can delete results."
                });
            }

            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Result deleted successfully."
            });
        }
    }
}
