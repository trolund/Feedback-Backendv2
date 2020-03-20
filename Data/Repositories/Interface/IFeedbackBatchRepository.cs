using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Feedback.Data_access.viewModels;
using Feedback.Models;
using Feedback.viewModels;

namespace Feedback.Data.Repositories {
    public interface IFeedbackBatchRepository : IRepository<FeedbackBatch> {

        Task<IEnumerable<FeedbackBatchDTO>> getAllFeedbackByMeetingId (int meetingId);

        Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId);

        Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData);
    }
}