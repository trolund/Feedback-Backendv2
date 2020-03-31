using System;
using System.Collections.Generic;

namespace Data.Models {
    public class QuestionSet : BaseEntity {
        public Guid QuestionSetId { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public List<Question> Questions { get; set; }

    }
}