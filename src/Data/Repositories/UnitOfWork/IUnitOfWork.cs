using System;
using System.Threading.Tasks;
using Data.Repositories.Interface;

namespace Data.Contexts {
    public interface IUnitOfWork : IDisposable {
        IMeetingRepository Meetings { get; }
        IQuestionSetRepository QuestionSet { get; }
        IFeedbackRepository Feedback { get; }
        IFeedbackBatchRepository FeedbackBatch { get; }
        ICompanyRepository Company { get; }
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IMeetingCategoryRepository MeetingCategories { get; }
        bool Save ();
        Task<bool> SaveAsync ();
    }
}