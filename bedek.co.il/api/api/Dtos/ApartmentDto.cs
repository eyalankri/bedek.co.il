using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models;
using Api.Dtos;


namespace api.Dtos
{
    public class ApartmentUpdateDto
    {


        [Required]
        public int ApartmentId { get; set; }
        
        [Required]
        public int ApartmentNumber { get; set; }

        [Required]        
        public DateTime DateOfEntrance { get; set; }

        public string Comment { get; set; }

        [Required]
        public UserDto User {get; set;}     
     
    }
}
