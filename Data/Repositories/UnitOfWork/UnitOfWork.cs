using System.ComponentModel;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Data.Contexts {
    public class UnitOfWork : IUnitOfWork {
        private readonly ApplicationDbContext _context;
        public IMeetingRepository Meetings { get; }
        public IQuestionSetRepository QuestionSet { get; }
        public IFeedbackRepository Feedback { get; }
        public IFeedbackBatchRepository FeedbackBatch { get; }
        public ICompanyRepository Company { get; }
        public IUserRepository Users { get; }
        public ICategoryRepository Categories { get; }
        public IMeetingCategoryRepository MeetingCategories { get; }

        public UnitOfWork (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMeetingRepository meetingRepository, IQuestionSetRepository questionSetRepository, IFeedbackRepository feedbackRepository, IFeedbackBatchRepository feedbackBatchRepository, ICompanyRepository companyRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, IMeetingCategoryRepository meetingCategoryRepository) {
            _context = context;
            Meetings = meetingRepository;
            QuestionSet = questionSetRepository;
            Feedback = feedbackRepository;
            Company = companyRepository;
            FeedbackBatch = feedbackBatchRepository;
            Users = userRepository;
            Categories = categoryRepository;
            MeetingCategories = meetingCategoryRepository;
        }
        public bool Save () {
            return (_context.SaveChanges () >= 0);
        }

        public async Task<bool> SaveAsync () {
            return (await _context.SaveChangesAsync () >= 0);
        }
        public void Dispose () {
            _context.Dispose ();
        }
    }
}