using System.Linq;

namespace BackEnd.Data
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Маппінг моделі доповідача із переліком сесій
        /// </summary>
        /// <param name="speaker"></param>
        /// <returns></returns>
        public static ConferenceDTO.SpeakerResponse MapSpeakerResponse(this Speaker speaker) =>
            new ConferenceDTO.SpeakerResponse
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Bio = speaker.Bio,
                WebSite = speaker.WebSite,
                Sessions = speaker.SessionSpeakers?
                    .Select(ss => new ConferenceDTO.Session
                    {
                        Id = ss.SessionId,
                        Title = ss.Session.Title
                    })
                    .ToList()
            };
    }
}