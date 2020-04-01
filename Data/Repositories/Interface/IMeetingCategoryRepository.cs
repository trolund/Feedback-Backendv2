using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.ViewModels;

namespace Data.Repositories.Interface {
    public interface IMeetingCategoryRepository : IRepository<MeetingCategory, Guid> {

        Task<List<MeetingCategory>> getAllMeetingCategoriesForMeeting (List<Guid> Ids);

        Task<List<MeetingCategory>> getMeetingCategoriesForMeeting (int meetingId);

    }
}