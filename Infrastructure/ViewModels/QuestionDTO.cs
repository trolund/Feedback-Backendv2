using System;
using System.Text.Json.Serialization;

namespace Infrastructure.ViewModels {
    public class QuestionDTO {
        public Guid QuestionId { get; set; }

        [JsonIgnore]
        public Guid QuestionSetId { get; set; }
        public string TheQuestion { get; set; }
    }
}