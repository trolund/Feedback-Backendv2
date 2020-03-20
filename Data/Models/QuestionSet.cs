using System;
using System.Collections.Generic;
using Feedback.Domain.Models;

namespace Feedback.Models {
    public class QuestionSet : BaseEntity {
        public Guid QuestionSetId { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public List<Question> Questions { get; set; }

    }
}