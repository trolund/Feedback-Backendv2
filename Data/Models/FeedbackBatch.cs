using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Feedback.Domain.Models;

namespace Feedback.Models {
    public class FeedbackBatch : BaseEntity {

        [Key]
        public Guid FeedbackBatchId { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        public List<Feedback> Feedback { get; set; }

    }
}