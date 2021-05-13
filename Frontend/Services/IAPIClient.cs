using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;

namespace Frontend.Services
{
    public interface IAPIClient
    {
        Task<List<SessionResponse>> GetSessionsAsync();
        Task<SessionResponse> GetSessionAsync(int id);
        Task<List<SpeakerResponse>> GetSpeakersAsync();
        Task<SpeakerResponse> GetSpeakerAsync(int id);
        Task<AttendeeResponse> GetAttendeeAsync(string name);
        Task PutSessionAsync(Session session);
        Task<bool> AddAttendeeAsync(string name);
        Task DeleteSessionAsync(int id);
        Task<List<SearchResult>> SearchAsync(string query);
    }
}