using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class UserInRole
    {
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
    }
}
