using System;
namespace Infrastructure.ViewModels
{
    public class LoginDTO
    {
        public LoginDTO()
        {
            

        }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
