using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ServiceCall
    {
        public ServiceCall()
        {                         
            DateUpdated = DateTime.Today;            
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid ServiceCallId { get; set; }
        [Required]
        public int ApartmentId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public string Status { get; set; }

    }
}
