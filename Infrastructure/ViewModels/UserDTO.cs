using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.ViewModels {
    public class UserDTO {

        public UserDTO (int companyId, string firstname, string lastname) {
            CompanyId = companyId;
            Firstname = firstname;
            Lastname = lastname;
        }

        public int CompanyId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        //public string Password { get; set; }
        public ICollection<RatingsDTO> Ratings { get; set; }
        public ICollection<string> Roles { get; set; }
        public string Token { get; set; }
    }
}