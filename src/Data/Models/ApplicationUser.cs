using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Data.Models {
    public class ApplicationUser : IdentityUser {

        public ApplicationUser () { }

        public int CompanyId { get; set; }

        [JsonIgnore]
        [Required]
        public Company Company { get; set; }

        [Required]
        public bool CompanyConfirmed { get; set; } = false;

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [JsonIgnore]
        public ICollection<Meeting> Meetings { get; set; }

        [JsonIgnore]
        public ICollection<Rating> Ratings { get; set; }

    }
}