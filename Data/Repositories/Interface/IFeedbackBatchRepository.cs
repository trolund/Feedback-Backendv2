using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.ViewModels;

namespace Data.Repositories.Interface {
    public interface IFeedbackBatchRepository : IRepository<FeedbackBatch, Guid> {

        Task<IEnumerable<FeedbackBatchDTO>> getAllFeedbackByMeetingId (int meetingId);

        Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId);

        Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData);

        Task<IEnumerable<FeedbackDateDTO>> OwnFeedbackDate (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData);
    }
}