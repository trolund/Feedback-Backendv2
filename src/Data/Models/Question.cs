using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models {
    public class Question : BaseEntity {
        public Question () { }
        public Question (Guid questionId, string theQuestion, int index) {
            QuestionId = questionId;
            TheQuestion = theQuestion;
            Index = index;
        }

        [Required]
        public int Index { get; set; }

        [MaxLength (80)]
        [Required]
        public string TheQuestion { get; set; }
        public Guid QuestionId { get; set; }
        public List<Feedback> Feedback { get; set; }

    }
}