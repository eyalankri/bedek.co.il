using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Service
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public int WarrantyPeriodInMonths { get;  set; }

        [Required]
        public bool? IsEnable { get; set; }

        public virtual ICollection<ServiceInHandyman> ServiceInUser { get; set; }
        public virtual ICollection<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }

    }
}
