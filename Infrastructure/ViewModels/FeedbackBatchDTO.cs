using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Feedback.Application;

namespace Feedback.viewModels {

    [FeedbackMustMatchQuestions]
    public class FeedbackBatchDTO {

        [JsonIgnore]
        public Guid FeedbackBatchId { get; set; }

        [Required]
        public string MeetingId { get; set; }

        [Required]
        public List<FeedbackDTO> Feedback { get; set; }
    }
}