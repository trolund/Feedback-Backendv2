using System;

namespace Infrastructure.ViewModels {
    public class MeetingCategoryDTO {
        public Guid MeetingCategoryId { get; set; }
        public int MeetingId { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

    }
}