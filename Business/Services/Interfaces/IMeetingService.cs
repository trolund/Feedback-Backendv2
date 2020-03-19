using System.Collections.Generic;
using System.Threading.Tasks;
using Feedback.Data_access.viewModels;
using Feedback.QueryParams;
using Feedback.viewModels;

namespace Feedback.Services.Interface {
    public interface IMeetingService {
        Task<MeetingDTO> GetMeeting (int id);

        Task<MeetingDTO> GetMeeting (string id);

        Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingResourceParameters parameters);

        Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingDateResourceParameters parameters);

        Task CreateMeeting (MeetingDTO meeting);

        Task<MeetingDTO> UpdateMeeting (MeetingDTO meeting);

        Task DeleteMeeting (MeetingDTO meeting);
        byte[] GetQRCode (string shortCodeId);

        Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId);
    }
}