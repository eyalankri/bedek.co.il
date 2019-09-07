using api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ServiceCall
    {
        public ServiceCall()
        {                         
            DateUpdated = DateCreated = DateTime.Today;            
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public Guid ServiceCallId { get; set; }

        [Required]
        public int ApartmentId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public ServiceCallStatus Status { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ServiceInHandymanInBuildingInServiceCall> ServiceInHandymanInBuildingInServiceCall { get; set; }
        public virtual ICollection<ServiceCallDoc> ServiceCallDoc { get; set; }
    }
}
