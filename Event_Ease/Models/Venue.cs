using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Event_Ease.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        public string VenueName { get; set; }

        [Required]
        public string Location { get; set; }

        public int Capacity { get; set; }

        public string ImageUrl { get; set; }

        //This property will hold a list of events that are associated with this venue
        
        public List<Event> Events { get; set; } = new();
    }
}
