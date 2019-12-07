using System;
using System.ComponentModel.DataAnnotations;

namespace NomoBucket.API.Dtos
{
    public class UserRegistrationDto

    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "Password must be between 4 and 8 characters")]
        public string Password { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime Created { get; set; }

        public UserRegistrationDto()
        {
            LastActive = DateTime.Now;
            Created = DateTime.Now;
        }
    }
}