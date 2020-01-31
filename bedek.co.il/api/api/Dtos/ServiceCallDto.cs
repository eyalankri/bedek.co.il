using api.Enums;
using Microsoft.AspNetCore.Http;
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
        public Guid ServiceCallDocId { get; set; }

        public Guid ServiceCallId { get; set; }

        //[Required]
        public int ApartmentId { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public DateTime DateUpdated { get; set; }
        
        public string Status { get; set; }
        


        public int ApartmentNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string ProjectName { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }

        public int[] ArrServiceInHandymanInBuildingId { get; set; }

        public virtual ICollection<ServiceInHandymanInBuildingInServiceCall> ServiceInHandymanInBuildingInServiceCall { get; set; }
        public virtual ICollection<ServiceCallDoc> ServiceCallDoc { get; set; }

        public string FileName { get; set; }
   

        public string FileContentType { get; set; }


        // [Required]
        public string DocDescription { get; set; }         

        //[Required]
        public IFormFile PostedFile { get; set; }
    }

    public class Test
    {
        public IFormFile PostedFile { get; set; }
    }
}
