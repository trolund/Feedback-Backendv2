using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.QueryParams;
using Infrastructure.ViewModels;

namespace Data.Repositories.Interface {
    public interface IMeetingRepository : IRepository<Meeting, int> {
        Task<Meeting> GetMeeting (int id, bool requireRole);
        Task<IEnumerable<Meeting>> GetMeetings (MeetingResourceParameters parameters);

        Task<IEnumerable<Meeting>> GetMeetings (MeetingDateResourceParameters parameters, string userId);

        Task<IEnumerable<Meeting>> GetMeetings (MeetingDateResourceParameters parameters, string userId, bool isVAdmin, bool isAdmin);

        void CreateMeeting (Meeting meeting);

        void UpdateMeeting (Meeting meeting);

        void DeleteMeeting (Meeting meeting);

        bool MeetingExists (int meetingId);

        Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId);

        Task<IEnumerable<Meeting>> GetMeetingsOneDay (DateTime date, string userId, bool requireRole);
    }
}