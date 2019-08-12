using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ServiceDto
    {
       
        public int ServiceId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public int WarrantyPeriodInMonths { get;  set; }

        [Required]
        public bool? IsEnable { get; set; }

        public  ICollection<ServiceInHandyman> ServiceInUser { get; set; }
        public  ICollection<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }
    }
}
