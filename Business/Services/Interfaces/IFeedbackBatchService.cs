using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface IFeedbackBatchService {
        Task<IEnumerable<FeedbackBatchDTO>> GetAllFeedbackBatchByMeetingId (string meetingShortId);
        Task<IEnumerable<FeedbackBatchDTO>> GetAllFeedbackBatch (string meetingShortId);
        Task<IEnumerable<FeedbackBatchDTO>> GetFeedbackBatchByMeetingId (string meetingId);
        Task<FeedbackBatchDTO> GetFeedbackBatch (string id);
        Task Create (FeedbackBatchDTO Feedback);
        Task<FeedbackBatchDTO> Update (FeedbackBatchDTO Feedback);
        Task Delete (FeedbackBatchDTO Feedback);

        Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord);

        Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, bool onlyOwnData);

    }
}