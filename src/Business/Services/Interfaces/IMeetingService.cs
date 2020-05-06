using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.QueryParams;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
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

        Task<IEnumerable<MeetingDTO>> GetMeetingsOneDay (DateTime date);

        Task<bool> IsMeetingOpenForFeedback (string id);
    }
}