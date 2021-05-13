using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session = BackEnd.Data.Session;
using Speaker = BackEnd.Data.Speaker;

namespace BackEnd.Controllers
{
    /// <summary>
    /// Отримання інформації про доповідачів
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeakersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Повертає усіх доповідачів
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SpeakerResponse>>> GetSpeakers()
        {
            var speakers = await _context.Speakers.AsNoTracking()//не відслідковувати зміни
                .Include(s => s.SessionSpeakers)
                .ThenInclude<Speaker, SessionSpeaker, Session>(ss => ss.Session)
                .Select(s => s.MapSpeakerResponse())
                .ToListAsync();
            
            return speakers;
        }

        /// <summary>
        /// Повертає конкретного доповідача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SpeakerResponse>> GetSpeaker(int id)
        {
            var speaker = await _context.Speakers.AsNoTracking()
                .Include(s => s.SessionSpeakers)
                .ThenInclude<Speaker, SessionSpeaker, Session>(ss => ss.Session)
                .SingleOrDefaultAsync(s => s.Id == id);
            if (speaker == null)
                return NotFound("Такий доповідач відсутній");

            return speaker.MapSpeakerResponse();
        }
    }
}
