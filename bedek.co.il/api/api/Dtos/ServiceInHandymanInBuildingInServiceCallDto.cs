using api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{

    public class ServiceCallDto
    {

        [Required]
        public int ApartmentId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public ServiceCallStatus Status { get; set; }

        public string Description { get; set; }

        public int[] ArrServiceInHandymanInBuildingId { get; set; }

       
    }




    public class ServiceInHandymanInBuildingInServiceCallDto
    {
        public int? ServiceInHandymanInBuildingId { get; set; }
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public string Company { get; set; }    
        public int BuildingId { get; set; }
        public int? ApartmentId { get; set; }
        public bool IsAssociated { get; set; }
        public int WarrantyPeriodInYears { get; set; }
        public DateTime DateOfEntrance { get; set; }
        public bool IsWarrantyExpired { get; set; }
        public double WarrantyDaysElpased { get; set; }

    }
}
