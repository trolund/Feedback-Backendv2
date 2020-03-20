using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Feedback.Data;
using Feedback.Data.Repositories;
using Feedback.Data_access.viewModels;
using Feedback.Domain.Models;
using Feedback.Models;
using Feedback.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Data_access.Repositories {
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        public MeetingRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        // public IEnumerable<Meeting> GetMeeting(int count)
        // {
        //     return context.Meetings.OrderByDescending(c => c.StartTime).Take(count).ToList();
        // }

        public async Task<IEnumerable<Meeting>> GetMeetings (MeetingResourceParameters parameters) {
            // throw if no parametres is provided.
            if (parameters == null) {
                throw new ArgumentNullException (nameof (parameters));
            }
            // filtering
            var collection = _context.Meetings as IQueryable<Meeting>;

            if (!string.IsNullOrWhiteSpace (parameters.MainCategory)) {
                var mainCategory = parameters.MainCategory.Trim ();
                collection = collection.Where (a => a.Topic == mainCategory);
            }

            if (!string.IsNullOrWhiteSpace (parameters.SearchQuery)) {
                var searchQuery = parameters.SearchQuery.Trim ();
                collection = collection.Where (m => m.Discription.Contains (searchQuery) ||
                    m.Name.Contains (searchQuery) ||
                    m.Topic.Contains (searchQuery) ||
                    m.CreatedBy.Contains (_httpContextAccessor.HttpContext.User.Identity.Name));
            }

            // pageing
            return await collection
                .Skip ((parameters.PageNumber - 1) * parameters.PageSize)
                .Take (parameters.PageSize)
                .ToListAsync ();
        }

        public async Task<Meeting> GetMeeting (int id) {
            return await _context.Meetings
                .Include (m => m.meetingCategories)
                .ThenInclude (x => x.Category)
                .SingleOrDefaultAsync (m => m.MeetingId == id);
        }

        public void CreateMeeting (Meeting meeting) {
            if (meeting == null) {
                throw new ArgumentNullException (nameof (meeting));
            }

            _context.Meetings.Add (meeting);
        }

        public void UpdateMeeting (Meeting meeting) {
            //_context.Entry (meeting).State = EntityState.Modified;
        }

        public void DeleteMeeting (Meeting meeting) {
            if (meeting == null) {
                throw new ArgumentNullException (nameof (Meeting));
            }

            _context.Remove (meeting);
        }

        public bool MeetingExists (int meetingId) {
            return _context.Meetings.Any (a => a.MeetingId == meetingId);
        }

        public async Task<IEnumerable<Meeting>> GetMeetings (MeetingDateResourceParameters parameters) {
            var MaxReturn = 500;
            // throw if no parametres is provided.
            if (parameters == null) {
                throw new ArgumentNullException (nameof (parameters));
            }
            // filtering
            var collection = _context.Meetings as IQueryable<Meeting>;

            if (parameters.Start != null && parameters.End != null) {

                collection = collection.Where (a => a.StartTime >= parameters.Start && a.StartTime <= parameters.End);
            }

            return await collection.Take (MaxReturn).ToListAsync ();
        }
        public async Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId) {
            return _mapper.Map<IEnumerable<CategoryDTO>> (await _context.Categories.Where (i => i.CompanyId == CompanyId).ToListAsync ());
        }
        // public bool MeetingExists(int id){
        //  return _context.Meetings.Where(m => m.MeetingId == id) != null ? true : false; 
        // }
    }
}