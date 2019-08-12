using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ApartmentDoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApartmentDocId { get; set; }

        [Required]
        public int ApartmentId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string DocDescription { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string FileName { get; set; }

        public DateTime DateUploaded { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FileContentType { get; set; }
       
        
    }


}
