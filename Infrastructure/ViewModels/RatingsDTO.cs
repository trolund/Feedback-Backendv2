using System;

namespace Feedback.Data_access.viewModels {
    public class RatingsDTO {
        public Guid RatingId { get; set; }
        public int rating { get; set; }
        public Guid ApplicationUserId { get; set; }

    }
}