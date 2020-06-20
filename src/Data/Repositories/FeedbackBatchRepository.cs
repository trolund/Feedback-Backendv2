using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Data.Repositories.Interface;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories {
    public class FeedbackBatchRepository : Repository<FeedbackBatch, Guid>, IFeedbackBatchRepository {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private readonly ILogger<FeedbackBatchRepository> _logger;

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public FeedbackBatchRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<FeedbackBatchRepository> logger) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackBatchDTO>> getAllFeedbackByMeetingId (int meetingId, bool requireRoles) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;

            collection = collection.Include (batch => batch.Feedback);

            if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN) || !requireRoles) {
                return _mapper.Map<IEnumerable<FeedbackBatchDTO>> (
                    await collection.Where (batch => batch.MeetingId == meetingId).ToListAsync ());
            } else {
                if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.VADMIN)) {
                    var companyId = Int32.Parse (_httpContextAccessor.HttpContext.User.FindFirstValue ("CID"));
                    collection = collection.Where (m => m.Meeting.ApplicationUser.CompanyId.Equals (companyId));
                } else {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue (ClaimTypes.NameIdentifier);
                    collection = collection.Where (m => m.Meeting.ApplicationUser.Id.Equals (userId));
                }
                return _mapper.Map<IEnumerable<FeedbackBatchDTO>> (
                    await collection.Where (batch => batch.MeetingId == meetingId).ToListAsync ());
            }
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
                collection = collection.Where (u => u.Meeting.ApplicationUserId.Equals (userId)); // TODO needs fixing?
            }

            if (userId == null && onlyOwnData) {
                throw new ArgumentException ("user not identifyed.");
            }

            if (companyId != null && !_httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN)) {
                collection = collection.Where (c => c.Meeting.ApplicationUser.CompanyId.Equals (Int32.Parse (companyId)));
            }

            // var test = collection.ToList ();

            if (categories != null && categories.Length > 0) {
                //collection = collection.Where (x => x.Meeting.meetingCategories.Any(x => x.Category.Name.Equals("Møder")));
                var catList = categories.ToList ();
                collection = collection.Where (x => x.Meeting.meetingCategories.Any (y => catList.Contains (y.CategoryId.ToString ())));

            }

            if (start != null && end != null) {
                // collection = collection.Where (a => a.CreatedDate >= start && a.CreatedDate <= end);
                // Filter on the date of the meeting the feedback batch is in relation to.
                collection = collection.Where (a => a.Meeting.StartTime >= start && a.Meeting.StartTime <= end);
            }

            if (searchWord != null) {
                collection = collection.Where (a => a.Meeting.Discription.Contains (searchWord) || a.Meeting.Name.Contains (searchWord));
            }

            try {
                var result = await collection.SelectMany (i => i.Feedback).Select (item => new FeedbackDateDTO (item.CreatedDate ?? DateTime.Now, item.Answer, item.FeedbackBatch.Meeting.meetingCategories.Select (i => i.Category.Name), item.QuestionId, item.FeedbackBatch.FeedbackBatchId, item.FeedbackBatch.Meeting.QuestionsSetId, MeetingIdHelper.GenerateShortId (item.FeedbackBatch.MeetingId))).ToListAsync ();
                return result;
            } catch (Exception e) {
                _logger.LogError ("OwnFeedbackDate faild, userid: " + userId, e);
                throw new DALException ("OwnFeedbackDate faild, userid: " + userId, e);
            }
        }

        public async Task<double> GetUserRating (string userId) {
            var collection = _context.FeedbackBatchs as IQueryable<FeedbackBatch>;
            collection = collection.Where (f => f.Meeting.CreatedBy.Equals (userId));
            try {
                // rating from the last 10 meeting
                var avg = await collection.SelectMany (f => f.Feedback, (f, g) => g).OrderByDescending (d => d.CreatedDate).Take (10).AverageAsync (f => f.Answer);
                // var oldavg = await collection.SelectMany (f => f.Feedback, (f, g) => g).OrderByDescending(d => d.CreatedDate).Skip(10).Take(10).AverageAsync (f => f.Answer);
                return avg * 2;
            } catch (InvalidOperationException e) { // intet feedback er givet endnu return 0
                return 0;
            } catch (Exception e) {
                _logger.LogError ("User rating faild, userid: " + userId, e);
                throw new DALException ("User rating faild, userid: " + userId, e);
            }
        }

        public async Task<List<FeedbackBatch>> getFeedbackByFingerprintandMeetingId (int meetingId, string fingerprint) {
            return await _context.FeedbackBatchs.Where (f => f.MeetingId.Equals (meetingId)).Where (f => f.UserFingerprint.Equals (fingerprint)).ToListAsync ();
        }

    }
}