using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace api.Dtos
{
    public class ApartmentDocDto
    {

        public int ApartmentDocId { get; set; }

        [Required]
        public int ApartmentId { get; set; }

        [Required]
        public int BuildingId { get; set; }


        [Required]
        public string DocDescription { get; set; }


        public string FileName { get; set; }

        public DateTime DateUploaded { get; set; }

        public string FileContentType { get; set; }

        [Required]
        public IFormFile PostedFile { get; set; }



    }


}
