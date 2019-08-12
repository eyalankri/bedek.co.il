using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ServiceInHandyman
    {
        
        
        [Required]
        public Guid UserId { get; set; }
        
        
        [Required]
        public int ServiceId { get; set; }
    }
}
