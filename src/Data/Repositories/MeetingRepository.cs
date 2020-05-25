using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Infrastructure.QueryParams;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class MeetingRepository : Repository<Meeting, int>, IMeetingRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;

        public MeetingRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

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

            _context.Meetings.Remove (meeting);
        }

        public bool MeetingExists (int meetingId) {
            return _context.Meetings.Any (a => a.MeetingId == meetingId);
        }

        public async Task<IEnumerable<Meeting>> GetMeetings (MeetingDateResourceParameters parameters, string userId) {
            var MaxReturn = 500;
            // throw if no parametres is provided.
            if (parameters == null) {
                throw new ArgumentNullException (nameof (parameters));
            }
            // filtering
            var collection = _context.Meetings as IQueryable<Meeting>;

            // return only the own users meetings
            if (userId == null) return null;
            collection = collection.Where (m => m.ApplicationUserId.Equals (userId)); // TODO need fix?

            if (parameters.Start != null && parameters.End != null) {

                collection = collection.Where (a => a.StartTime >= parameters.Start && a.StartTime <= parameters.End);
            }

            return await collection.Take (MaxReturn).ToListAsync ();
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsOneDay (DateTime date, string userId) {
            // filtering
            var collection = _context.Meetings as IQueryable<Meeting>;

            // return only the own users meetings
            if (userId == null) return null;
            if (date == null) return null;

            collection = collection.Where (m => m.ApplicationUserId.Equals (userId)); // TODO need fix?

            collection = collection.Where (a => a.StartTime.Day.Equals (date.Day) && a.StartTime.Month.Equals (date.Month) && a.StartTime.Year.Equals (date.Year));

            return await collection
                .OrderByDescending (d => d.StartTime.TimeOfDay)
                .ToListAsync ();
        }
        public async Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId) {
            return _mapper.Map<IEnumerable<CategoryDTO>> (await _context.Categories.Where (i => i.CompanyId == CompanyId || i.CompanyId == 1).ToListAsync ());
        }
        // public bool MeetingExists(int id){
        //  return _context.Meetings.Where(m => m.MeetingId == id) != null ? true : false; 
        // }
    }
}