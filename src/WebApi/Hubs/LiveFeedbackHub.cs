using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Data.Models;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs {

    [Authorize]
    public class LiveFeedbackHub : Hub {
        private readonly IFeedbackBatchService _service;
        private readonly IMeetingService _meetingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LiveFeedbackHub (IFeedbackBatchService service, UserManager<ApplicationUser> userManager, IMeetingService meetingService) {
            _service = service;
            _userManager = userManager;
            _meetingService = meetingService;
        }

        public async Task JoinRoom (string meetingId) {
            var user = _userManager.Users.SingleOrDefault (u => u.Email == Context.User.Identity.Name);
            var meeting = await _meetingService.GetMeeting (MeetingIdHelper.GetId (meetingId));

            // Users own meeting
            var sub = Context.User.Claims.Where (c => c.Type.Equals (ClaimTypes.NameIdentifier)).First ().Value;
            if (meeting.CreatedBy.Equals (sub)) {
                await Groups.AddToGroupAsync (Context.ConnectionId, meetingId);
                return;
            }

            // VAdmin for the firm how employies the user who created the meeting.
            var userWhoCreatedMeeting = _userManager.Users.SingleOrDefault (u => u.Id == meeting.CreatedBy);
            var companyId = Context.User.Claims.Where (c => c.Type.Equals ("CID")).First ().Value;
            if (userWhoCreatedMeeting.CompanyId.Equals (companyId)) {
                await Groups.AddToGroupAsync (Context.ConnectionId, meetingId);
                return;
            }
        }

        public Task LeaveRoom (string meetingId) {
            return Groups.RemoveFromGroupAsync (Context.ConnectionId, meetingId);
        }
    }
}