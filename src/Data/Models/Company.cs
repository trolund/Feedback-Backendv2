using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models {

    public class Company : BaseEntity {
        public int CompanyId { get; set; }

        [Required]
        [MaxLength (150)]
        public string Name { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}