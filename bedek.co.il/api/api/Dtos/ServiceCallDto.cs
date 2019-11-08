using api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ServiceCallDto
    {
        public ServiceCallDto()
        {
            DateUpdated = DateCreated = DateTime.Today;
        }


        public Guid ServiceCallId { get; set; }

        [Required]
        public int ApartmentId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public string Status { get; set; }
        


        public int ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string ProjectName { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ServiceInHandymanInBuildingInServiceCall> ServiceInHandymanInBuildingInServiceCall { get; set; }
        public virtual ICollection<ServiceCallDoc> ServiceCallDoc { get; set; }
    }
}
