using System.Collections.Generic;

namespace BackEnd.Data
{
    public class Attendee : ConferenceDTO.Attendee
    {
        public virtual ICollection<SessionAttendee> SessionAttendees { get; set; }
    }
}