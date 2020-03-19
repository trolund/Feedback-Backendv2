using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.ViewModels {
    public class UserRegistrationDTO {

        public int CompanyId { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public ICollection<string> RequesetedRoles { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordAgain { get; set; }

        [Required]
        public string Phone { get; set; }
        public CompanyDTO company { get; set; }
    }
}