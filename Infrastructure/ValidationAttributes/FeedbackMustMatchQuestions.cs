using System.ComponentModel.DataAnnotations;

namespace Feedback.Application {
    public class FeedbackMustMatchQuestions : ValidationAttribute {

        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {

            //var _meetingService = (IMeetingService) validationContext.GetService (typeof (IMeetingService));
            //var _questionSetService = (IQuestionSetService) validationContext.GetService (typeof (IQuestionSetService));

            //var feedbackBatch = (FeedbackBatchDTO) validationContext.ObjectInstance;
            //var meeting = _meetingService.GetMeeting (feedbackBatch.MeetingId).Result;

            //var questionSet = _questionSetService.GetQuestionSet (meeting.QuestionsSetId).Result;

            //if (feedbackBatch.Feedback.ToList ().Count != questionSet.Questions.ToList ().Count) {
            //    return new ValidationResult ("Number of feedback votes does not match question set", new [] { nameof (feedbackBatch) });
            //}

            return ValidationResult.Success;
        }

    }
}