using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.ViewModels;

namespace Data.Contexts.Repositories {
    public interface IFeedbackBatchRepository : IRepository<FeedbackBatch> {

        Task<IEnumerable<FeedbackBatchDTO>> getAllFeedbackByMeetingId (int meetingId);

        Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId);

        Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData);
    }
}