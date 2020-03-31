using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.ViewModels {
    public class FeedbackDTO {
        [JsonIgnore]
        public string FeedbackId { get; set; }

        [JsonIgnore]
        public string feedbackBatchId { get; set; }

        [Required]
        public string QuestionId { get; set; }

        [Range (0, 3, ErrorMessage = "Please enter valid integer Number between 0 and 3")]
        public int Answer { get; set; }

        [MaxLength (300)]
        public string Comment { get; set; }

    }
}