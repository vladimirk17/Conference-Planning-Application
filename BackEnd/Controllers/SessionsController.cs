using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session = BackEnd.Data.Session;
using Speaker = BackEnd.Data.Speaker;

namespace BackEnd.Controllers
{
    /// <summary>
    /// Контроллер для керування доповідями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Повертає всі доповіді
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SessionResponse>>> GetAll()
        {
            var sessions = await _context.Sessions.AsNoTracking()
                .Include(s => s.Track)
                .Include(s => s.SessionSpeakers)
                .ThenInclude<Session, SessionSpeaker, Speaker>(ss => ss.Speaker)
                .Select(m => m.MapSessionResponse())
                .ToListAsync();
            return sessions;
        }
        
        /// <summary>
        /// Повертає конкретну доповідь
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionResponse>> Get(int id)
        {
            var session = await _context.Sessions.AsNoTracking()
                .Include(s => s.Track)
                .Include(s => s.SessionSpeakers)
                .ThenInclude<Session, SessionSpeaker, Speaker>(ss => ss.Speaker)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (session == null)
                return NotFound();

            return session.MapSessionResponse();
        }
        
        /// <summary>
        /// Додати доповідь
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SessionResponse>> Post(ConferenceDTO.Session input)
        {
            var session = new Session
            {
                Title = input.Title,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Abstract = input.Abstract,
                TrackId = input.TrackId
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var result = session.MapSessionResponse();

            return CreatedAtAction(nameof(GetAll), new {id = result.Id}, result);
        }

        /// <summary>
        /// змінити доповідь
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ConferenceDTO.Session input)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
                return NotFound();

            session.Id = input.Id;
            session.Title = input.Title;
            session.Abstract = input.Abstract;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;
            session.TrackId = input.TrackId;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Видалити доповідь
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<SessionResponse>> Delete(int id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
                return NotFound();

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return session.MapSessionResponse();
        }
    }
}