using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models;
using Api.Dtos;


namespace api.Dtos
{
    public class ApartmentDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ApartmentId { get; set; }

        public int BuildingId { get; set; }

        [Required]
        public int ApartmentNumber { get; set; }
        [Required]
        public DateTime DateOfEntrance { get; set; }

        public string Comment { get; set; }

        public UserDto User { get; set; }

        public ICollection<ApartmentDoc> ApartmentDocs { get; set; }



        public string City { get; set; }
        
        public string Street { get; set; }
              
        public string BuildingNumber { get; set; }

        public string ProjectName { get; set; }

       


    }
}
