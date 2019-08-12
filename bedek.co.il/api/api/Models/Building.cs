using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Building
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuildingId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Street { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Required]
        public string BuildingNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ProjectName { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }


    }

   
}
