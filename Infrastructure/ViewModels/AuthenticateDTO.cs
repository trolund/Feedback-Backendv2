using System.ComponentModel.DataAnnotations;

namespace Infrastructure.ViewModels {
    public class AuthenticateDTO {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}