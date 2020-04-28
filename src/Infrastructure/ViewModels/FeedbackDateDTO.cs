using System;
using System.Collections.Generic;

namespace Infrastructure.ViewModels {
    public class FeedbackDateDTO {
        public FeedbackDateDTO (DateTime Date, int answer, IEnumerable<string> categories, Guid questionId, Guid feedbackBatchId, Guid questionSetId) {
            this.Date = Date;
            Answer = answer;
            Categories = categories;
            QuestionId = questionId;
            FeedbackBatchId = feedbackBatchId;
            QuestionSetId = questionSetId;
        }

        public DateTime Date { get; set; }
        public int Answer { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public Guid QuestionId { get; set; }
        public Guid FeedbackBatchId { get; set; }
        public Guid QuestionSetId { get; set; }
    }
}