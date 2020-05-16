using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models {
    public class Category : BaseEntity {
        public Guid CategoryId { get; set; }

        [Required]
        public bool active { get; set; }

        [Required]
        [MaxLength (100)]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public Company Company { get; set; }
        public ICollection<MeetingCategory> meetingCategories { get; set; }
    }
}