using System;
using System.Collections.Generic;
using Feedback.Data_access.viewModels;

namespace Feedback.viewModels {
    public class UserDTO {

        public UserDTO (int companyId, string firstname, string lastname) {
            CompanyId = companyId;
            Firstname = firstname;
            Lastname = lastname;
        }

        public int CompanyId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ICollection<RatingsDTO> Ratings { get; set; }
    }
}