using System;
using System.Collections.Generic;
using Feedback.Models;

namespace Feedback.Domain.Models {
    public class Category : BaseEntity {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<MeetingCategory> meetingCategories { get; set; }
    }
}