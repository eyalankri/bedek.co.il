using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ServiceInHandymanInBuilding
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int BuildingId { get; set; }
    }
}
