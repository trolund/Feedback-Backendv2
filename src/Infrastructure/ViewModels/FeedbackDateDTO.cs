using System;
using System.Collections.Generic;

namespace Infrastructure.ViewModels {
    public class FeedbackDateDTO {
        public FeedbackDateDTO (DateTime Date, int answer, IEnumerable<string> categories) {
            this.Date = Date;
            Answer = answer;
            Categories = categories;
        }

        public DateTime Date { get; set; }
        public int Answer { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}