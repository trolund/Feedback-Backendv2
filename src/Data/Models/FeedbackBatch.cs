using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models {
    public class FeedbackBatch : BaseEntity {

        [Key]
        public Guid FeedbackBatchId { get; set; }

        [Required]
        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public List<Feedback> Feedback { get; set; }

        [Required]
        public String UserFingerprint { get; set; }

        public Guid QuestionSetId { get; set; }

        [JsonIgnore]
        public QuestionSet QuestionSet { get; set; }

    }
}