using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Dtos
{
    public class BuildingDto
    {
        public int BuildingId { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string BuildingNumber { get; set; }

        public string ProjectName { get; set; }

        public ICollection<Apartment> Apartments { get; set; }
        public  ICollection<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }
    }
}
