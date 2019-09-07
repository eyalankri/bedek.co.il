using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ServiceInHandymanInBuildingInServiceCall
    {
        [Required]
        public int ServiceInHandymanInBuildingId { get; set; }

        [Required]
        public Guid ServiceCallId { get; set; }
    }
}
