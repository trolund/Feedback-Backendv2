using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class MeetingCategoryRepository : Repository<MeetingCategory, Guid>, IMeetingCategoryRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;
        public MeetingCategoryRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<List<MeetingCategory>> getAllMeetingCategoriesForMeeting (List<Guid> Ids, int MeetingId) {
            var collection = _context.Categories as IQueryable<MeetingCategory>;

            foreach (Guid id in Ids) {
                collection.Where (item => item.CategoryId == id && item.MeetingId == MeetingId);
            }

            return await collection.ToListAsync ();
        }

        public async Task<List<MeetingCategory>> getMeetingCategoriesForMeeting (int meetingId) {
            return await _context.MeetingCategories.Where (item => item.MeetingId == meetingId).ToListAsync ();
        }

    }

}