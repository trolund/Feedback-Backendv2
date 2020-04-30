using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.ViewModels {
    public class QuestionDTO {
        public Guid QuestionId { get; set; }

        [Required]
        public int Index { get; set; }

        [JsonIgnore]
        public Guid QuestionSetId { get; set; }

        [MaxLength (80)]
        public string TheQuestion { get; set; }
    }
}