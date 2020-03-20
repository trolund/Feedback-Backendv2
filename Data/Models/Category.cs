using System;
using System.Collections.Generic;

namespace Data.Models {
    public class Category : BaseEntity {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<MeetingCategory> meetingCategories { get; set; }
    }
}