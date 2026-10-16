
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Enrolments")]
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EventID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ParticipantID { get; set; }

        [Required]
        public DateTime EnrolmentDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string RaceNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = string.Empty;

        [ForeignKey(nameof(EventID))]
        public Event? Event { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }

        [ForeignKey(nameof(ParticipantID))]
        public RaceDayUser? Participant { get; set; }
    }
}
