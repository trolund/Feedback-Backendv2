using System;

namespace Data.Models {
    public class Rating : BaseEntity {
        public Guid RatingId { get; set; }
        public int rating { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}