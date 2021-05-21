using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    public class Session
    {
        public int Id { get; set; }
        [Required] [StringLength(200)] public string Title { get; set; }
        [StringLength(4000)] public virtual string Abstract { get; set; }
        public virtual DateTimeOffset? StartTime { get; set; }
        public virtual DateTimeOffset? EndTime { get; set; }
        public int? TrackId { get; set; }

        
        //Обчислення тривалості конференції із використанням null-coalescence оператора
        public TimeSpan Duration =>
            EndTime?.Subtract(StartTime ?? EndTime ?? DateTimeOffset.MinValue) ?? TimeSpan.Zero;
    }
}