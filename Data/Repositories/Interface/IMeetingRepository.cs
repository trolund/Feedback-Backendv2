using System.Collections.Generic;
using System.Threading.Tasks;
using Feedback.Data_access.viewModels;
using Feedback.Models;
using Feedback.QueryParams;

namespace Feedback.Data.Repositories {
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