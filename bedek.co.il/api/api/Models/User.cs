using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }        

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "char(64)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Phone1 { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Phone2 { get; set; }

        public int IdentityCardId { get; set; }

        [Required]
        public bool IsAcceptEmails { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [MaxLength(300)]
        public string Company { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }


        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        // service in handyman
        public virtual ICollection<ServiceInHandyman> ServiceInUser { get; set; }

        public virtual ICollection<ServiceInHandymanInBuilding> ServiceInHandymanInBuilding { get; set; }
    }
}
