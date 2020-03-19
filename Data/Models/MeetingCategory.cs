using System;

namespace Data.Models {
    public class MeetingCategory {
        // public Guid MeetingCategoryId { get; set; }
        public int MeetingId { get; set; }
        public Meeting meeting { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}