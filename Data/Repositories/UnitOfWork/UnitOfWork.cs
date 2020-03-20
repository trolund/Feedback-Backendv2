﻿using System.Threading.Tasks;
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
        public UnitOfWork (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) {
            _context = context;
            Meetings = new MeetingRepository (_context, httpContextAccessor, mapper);
            QuestionSet = new QuestionSetRepository (_context);
            Feedback = new FeedbackRepository (_context, httpContextAccessor);
            Company = new CompanyRepository (_context, httpContextAccessor);
            FeedbackBatch = new FeedbackBatchRepository (_context, httpContextAccessor, mapper);
            Users = new UserRepository (_context, httpContextAccessor, mapper);
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