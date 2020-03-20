using System;
using Feedback.Models;

namespace Feedback.Domain.Models {
    public class Rating : BaseEntity {
        public Guid RatingId { get; set; }
        public int rating { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}