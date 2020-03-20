using System;
using System.Collections.Generic;

namespace Infrastructure.ViewModels {
    public class QuestionSetDTO {
        public Guid QuestionSetId { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}