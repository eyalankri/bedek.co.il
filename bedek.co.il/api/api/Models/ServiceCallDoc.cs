using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ServiceCallDoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid ServiceCallDocId { get; set; }
        [Required]
        public Guid ServiceCallId { get; set; }

        public string DocDescription { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileContentType { get; set; }
        [Required]
        public int ServiceInHandymanInBuildingId { get; set; }
        [Required]
        public string Status { get; set; }

    }
}
