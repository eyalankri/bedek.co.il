using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models;
using Api.Utilities;

namespace Api.Dtos
{
    public class UserDto
    {

        public UserDto()
        {
            Password = Encryption.Sha256(RandomKey.GeneratePassword(5, 2, 2));
        }

        public Guid UserId { get; set; }

        [Required]        
        public string FirstName { get; set; }

        [Required]        
        public string LastName { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        [Required]        
        public string Password { get; set; }

        [Required]        
        public string Phone1 { get; set; }
        
        public string Phone2 { get; set; }

        public string IdentityCardId { get; set; }
        
        public bool IsAcceptEmails { get; set; }

        public bool IsDeleted { get; set; }

        public string Company { get; set; }       
        
        public string Comment { get; set; }

        public DateTime DateRegistered { get; set; }


    }
}
