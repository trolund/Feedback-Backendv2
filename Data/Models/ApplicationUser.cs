using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.Models {
    public class ApplicationUser : IdentityUser {

        public ApplicationUser () { }

        public ApplicationUser (string id) {
            base.Id = id;
        }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public bool CompanyConfirmed { get; set; } = false;
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Phone { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

        public ICollection<Rating> Ratings { get; set; }

    }
}