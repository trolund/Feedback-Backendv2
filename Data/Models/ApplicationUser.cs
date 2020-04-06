using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Data.Models {
    public class ApplicationUser : IdentityUser {

        public ApplicationUser () { }

        public ApplicationUser (string id) {
            base.Id = id;
        }

        public int CompanyId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Company Company { get; set; }
        public bool CompanyConfirmed { get; set; } = false;
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Meeting> Meetings { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Rating> Ratings { get; set; }

    }
}