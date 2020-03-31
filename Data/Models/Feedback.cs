using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models
{
    public class Feedback : BaseEntity
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public FeedbackBatch FeedbackBatch { get; set; }

        [Range (0, 3, ErrorMessage = "Please enter valid integer Number between 0 and 3")]
        public int Answer { get; set; }

        [MaxLength (300)]
        public string Comment { get; set; }

        [ForeignKey ("QuestionId")]
        public Guid QuestionId { get; set; }

        [JsonIgnore]
        public Question Question { get; set; }
    }
}