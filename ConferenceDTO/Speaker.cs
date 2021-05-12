using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    public class Speaker
    {
        public int Id { get; set; }
        /// <summary>
        /// Ім'я спікера
        /// </summary>
        [Required] [StringLength(200)] public string Name { get; set; }
        /// <summary>
        /// Біографія спікера
        /// </summary>
        [StringLength(4000)] public string Bio { get; set; }
        [StringLength(1000)] public virtual string WebSite { get; set; }
    }
}