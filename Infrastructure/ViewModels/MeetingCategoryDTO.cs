using System;

namespace Infrastructure.ViewModels {
    public class MeetingCategoryDTO {
        public string MeetingId { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

    }
}