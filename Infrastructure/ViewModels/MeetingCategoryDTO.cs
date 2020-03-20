using System;

namespace Feedback.Data_access.viewModels {
    public class MeetingCategoryDTO {

        public int MeetingId { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

    }
}