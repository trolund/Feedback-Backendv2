using System;
using System.Collections.Generic;

namespace Data.Models {
    public class Question : BaseEntity {
        public Question () { }
        public Question (Guid questionId, string theQuestion) {
            QuestionId = questionId;
            TheQuestion = theQuestion;
        }

        public string TheQuestion { get; set; }
        public Guid QuestionId { get; set; }
        public List<Feedback> Feedback { get; set; }

    }
}