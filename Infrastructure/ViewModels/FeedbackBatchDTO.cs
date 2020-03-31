using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.ValidationAttributes;

namespace Infrastructure.ViewModels {

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