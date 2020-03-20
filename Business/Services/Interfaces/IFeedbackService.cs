using System.Collections.Generic;
using Feedback.viewModels;

namespace Feedback.Services.Interface
{
    public interface IFeedbackService
    {
        FeedbackDTO getFeedback(int id);

        IEnumerable<FeedbackDTO> GetFeedback(string feedbackId);

        void CreateFeedback(FeedbackDTO Feedback);

        FeedbackDTO UpdateFeedback(FeedbackDTO Feedback);

        void DeleteFeedback(FeedbackDTO Feedback);
    }
}