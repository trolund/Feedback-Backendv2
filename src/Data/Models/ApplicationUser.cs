using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Data.Models {
    public class ApplicationUser : IdentityUser {

        public ApplicationUser () { }

        // [Key]
        // [DatabaseGeneratedAttribute (DatabaseGeneratedOption.Identity)]
        // public override Guid Id {
        //     get { return base.Id; }
        //     set { base.Id = value; }
        // }

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
        // [IgnoreDataMember]
        public ICollection<Meeting> Meetings { get; set; }

        [JsonIgnore]
        // [IgnoreDataMember]
        public ICollection<Rating> Ratings { get; set; }

    }
}