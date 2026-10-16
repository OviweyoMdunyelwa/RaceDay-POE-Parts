
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        public int OrganiserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000)]
        public decimal Distance { get; set; }

        [Required]
        [MaxLength(50)]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationDeadline { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("OrganiserID")]
        public RaceDayUser? Organiser { get; set; }
    }
}
