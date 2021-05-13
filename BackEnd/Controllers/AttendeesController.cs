using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attendee = ConferenceDTO.Attendee;
using Session = BackEnd.Data.Session;

namespace BackEnd.Controllers
{
    /// <summary>
    /// CRUD операції із учасниками
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Повертає дані учасника і доповіді, на які він зареєструвався
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<AttendeeResponse>> Get(string username)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                .ThenInclude<Data.Attendee, SessionAttendee, Session>(sa => sa.Session)
                .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
                return NotFound();

            var result = attendee.MapAttendeeResponse();

            return result;
        }
        
        /// <summary>
        /// Додати нового учасника
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AttendeeResponse>> Post(Attendee input)
        {
            // Перевірка чи вже було додано такого учасника 
            var existingAttendee = await _context.Attendees
                .Where(a => a.UserName == input.UserName)
                .FirstOrDefaultAsync();

            if (existingAttendee != null)
                return Conflict(input);

            var attendee = new Data.Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return CreatedAtAction(nameof(Get), new {username = result.UserName}, result);
        }

        /// <summary>
        /// Додати слухача доповіді
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpPost("{username}/session/{sessionId}")]
        public async Task<ActionResult<AttendeeResponse>> AddToSession(string username, int sessionId)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                .ThenInclude<Data.Attendee, SessionAttendee, Session>(sa => sa.Session)
                .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
                return NotFound();

            var session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
                return BadRequest();

            attendee.SessionAttendees.Add(new SessionAttendee
            {
                AttendeeId = attendee.Id,
                SessionId = sessionId
            });

            await _context.SaveChangesAsync();

            var result = attendee.MapAttendeeResponse();

            return result;
        }

        /// <summary>
        /// Видалити слухача доповіді
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpDelete("{username}/session/{sessionId}")]
        public async Task<IActionResult> RemoveFromSession(string username, int sessionId)
        {
            var attendee = await _context.Attendees.Include(a => a.SessionAttendees)
                .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
                return NotFound();

            var session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
                return BadRequest();

            var sessionAttendee = attendee.SessionAttendees.FirstOrDefault(sa => sa.SessionId == sessionId);
            attendee.SessionAttendees.Remove(sessionAttendee);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}