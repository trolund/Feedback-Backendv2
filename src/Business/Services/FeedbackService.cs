using System.Collections.Generic;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;

namespace Business.Services {
    public class FeedbackService : IFeedbackService {
        public void CreateFeedback (FeedbackDTO Feedback) {
            throw new System.NotImplementedException ();
        }

        public void DeleteFeedback (FeedbackDTO Feedback) {
            throw new System.NotImplementedException ();
        }

        public FeedbackDTO getFeedback (int id) {
            throw new System.NotImplementedException ();
        }

        public IEnumerable<FeedbackDTO> GetFeedback (string feedbackId) {
            throw new System.NotImplementedException ();
        }

        public IEnumerable<FeedbackDTO> GetFeedbackByMeetingId (string meetingId) {
            throw new System.NotImplementedException ();
        }

        public FeedbackDTO UpdateFeedback (FeedbackDTO Feedback) {
            throw new System.NotImplementedException ();
        }
    }
}