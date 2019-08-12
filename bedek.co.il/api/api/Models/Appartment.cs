using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Apartment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApartmentId { get; set; }

        [Required]
        public int BuildingId { get; set; }

        [Required]
        public int ApartmentNumber { get; set; }

        [Required]
        public DateTime DateOfEntrance { get; set; }

        public string Comment { get; set; }

        public virtual User User {get; set;}     
        
        public virtual ICollection<ApartmentDoc> ApartmentDocs { get; set; }
    }
}
