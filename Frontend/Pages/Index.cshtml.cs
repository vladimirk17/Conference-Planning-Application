using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAPIClient _apiClient;

        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }
        public IEnumerable<(int Offset, DayOfWeek? DayOfWeek)> DayOffsets { get; set; }
        public int CurrentDayOffset { get; set; }
        
        public IndexModel(IAPIClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGet(int day = 0)
        {
            CurrentDayOffset = day;

            var sessions = await _apiClient.GetSessionsAsync();
            var startDate = sessions.Min(s => s.StartTime?.Date);

            var offset = 0;
            DayOffsets = sessions
                .Select(s => s.StartTime?.Date)
                .Distinct()
                .OrderBy(d => d)
                .Select(day => (offset++, day?.DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            Sessions = sessions
                .Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(g => g.Key);
        }
    }
}
