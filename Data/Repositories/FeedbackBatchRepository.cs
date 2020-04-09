using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class FeedbackBatchRepository : Repository<FeedbackBatch, Guid>, IFeedbackBatchRepository {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public FeedbackBatchRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackBatchDTO>> getAllFeedbackByMeetingId (int meetingId) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;

            return _mapper.Map<IEnumerable<FeedbackBatchDTO>> (await collection
                .Include (batch => batch.Feedback)
                .Where (batch => batch.MeetingId == meetingId)
                .ToListAsync ());
        }

        public async Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;

            if (userId != null) {
                collection.Where (u => u.ModifiedBy.Equals (userId));
            }

            if (companyId != null) {
                collection.Where (c => c.Meeting.ApplicationUser.CompanyId.Equals (companyId));
            }

            if (categories != null && categories.Length > 0) {
                //collection = collection.Where (x => x.Meeting.meetingCategories.Any(x => x.Category.Name.Equals("Møder")));
                foreach (string i in categories) {
                    collection = collection.Where (a => a.Meeting.meetingCategories.Any (x => x.Category.Name.Contains (i)));
                }
            }

            if (start != null && end != null) {
                collection = collection.Where (a => a.CreatedDate >= start && a.CreatedDate <= end);
            }

            if (searchWord != null) {
                collection = collection.Where (a => a.Meeting.Discription.Contains (searchWord) || a.Meeting.Name.Contains (searchWord));
            }

            var result = await collection
                .Include (batch => batch.Feedback)
                .ToListAsync ();

            return _mapper.Map<IEnumerable<FeedbackBatchDTO>> (result);
        }

        public async Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;

            if (userId != null && onlyOwnData) {
                collection.Where (u => u.ModifiedBy.Equals (userId));
            }

            if (userId == null && !onlyOwnData) {
                throw new ArgumentException ("user not identifyed.");
            }

            if (companyId != null) {
                collection.Where (c => c.Meeting.ApplicationUser.CompanyId.Equals (companyId));
            }

            if (categories != null && categories.Length > 0) {
                //collection = collection.Where (x => x.Meeting.meetingCategories.Any(x => x.Category.Name.Equals("Møder")));
                foreach (string i in categories) {
                    collection = collection.Where (a => a.Meeting.meetingCategories.Any (x => x.Category.Name.Contains (i)));
                }
            }

            if (start != null && end != null) {
                collection = collection.Where (a => a.CreatedDate >= start && a.CreatedDate <= end);
            }

            if (searchWord != null) {
                collection = collection.Where (a => a.Meeting.Discription.Contains (searchWord) || a.Meeting.Name.Contains (searchWord));
            }

            var result = await collection.SelectMany (i => i.Feedback).Select (item => new FeedbackMonthDTO (item.CreatedDate.Value.Month, item.Answer, item.FeedbackBatch.Meeting.meetingCategories.Select (i => i.Category.Name))).ToListAsync ();

            return result;
        }

        public async Task<IEnumerable<FeedbackDateDTO>> OwnFeedbackDate (DateTime start, DateTime end, string[] categories, string searchWord, string userId, string companyId, bool onlyOwnData) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;

            if (userId != null && onlyOwnData) {
                collection = collection.Where (u => u.Meeting.ApplicationUserId.Equals (Guid.Parse (userId)));
            }

            if (userId == null && onlyOwnData) {
                throw new ArgumentException ("user not identifyed.");
            }

            if (companyId != null) {
                // collection = collection.Where (c => c.Meeting.ApplicationUser.CompanyId.Equals (Int32.Parse (companyId)));
            } else {
                return null;
            }

            if (categories != null && categories.Length > 0) {
                //collection = collection.Where (x => x.Meeting.meetingCategories.Any(x => x.Category.Name.Equals("Møder")));
                foreach (string catId in categories) {
                    collection = collection.Where (a => a.Meeting.meetingCategories.Any (item => item.CategoryId.Equals (Guid.Parse (catId))));
                }
            }

            if (start != null && end != null) {
                collection = collection.Where (a => a.CreatedDate >= start && a.CreatedDate <= end);
            }

            if (searchWord != null) {
                collection = collection.Where (a => a.Meeting.Discription.Contains (searchWord) || a.Meeting.Name.Contains (searchWord));
            }

            var result = await collection.SelectMany (i => i.Feedback).Select (item => new FeedbackDateDTO (item.CreatedDate ?? DateTime.Now, item.Answer, item.FeedbackBatch.Meeting.meetingCategories.Select (i => i.Category.Name))).ToListAsync ();

            return result;
        }

    }
}