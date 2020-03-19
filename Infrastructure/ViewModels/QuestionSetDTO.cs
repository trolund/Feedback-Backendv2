using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Feedback.Models;

namespace Feedback.viewModels {
    public class QuestionSetDTO {
        public Guid QuestionSetId { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}