using System;

namespace Infrastructure.ViewModels {
    public class RatingsDTO {
        public Guid RatingId { get; set; }
        public int rating { get; set; }
        public Guid ApplicationUserId { get; set; }

    }
}