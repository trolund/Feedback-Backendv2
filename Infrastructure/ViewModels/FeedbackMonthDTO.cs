using System.Collections.Generic;

namespace Feedback.viewModels {
    public class FeedbackMonthDTO {
        public FeedbackMonthDTO (int month, int answer, IEnumerable<string> categories) {
            Month = month;
            Answer = answer;
            Categories = categories;
        }

        public int Month { get; set; }
        public int Answer { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}