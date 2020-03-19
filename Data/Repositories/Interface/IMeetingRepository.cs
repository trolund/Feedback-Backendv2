using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.QueryParams;
using Infrastructure.ViewModels;

namespace Data.Contexts.Repositories {
    public interface IMeetingRepository : IRepository<Meeting> {
        Task<Meeting> GetMeeting (int id);
        Task<IEnumerable<Meeting>> GetMeetings (MeetingResourceParameters parameters);

        Task<IEnumerable<Meeting>> GetMeetings (MeetingDateResourceParameters parameters);

        void CreateMeeting (Meeting meeting);

        void UpdateMeeting (Meeting meeting);

        void DeleteMeeting (Meeting meeting);

        bool MeetingExists (int meetingId);

        Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId);
    }
}