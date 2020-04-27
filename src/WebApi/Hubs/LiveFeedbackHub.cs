using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs {
    public class LiveFeedbackHub : Hub {
        private readonly IFeedbackBatchService _service;

        public LiveFeedbackHub (IFeedbackBatchService service) {
            _service = service;
        }

        public Task JoinRoom (string meetingId) {
            return Groups.AddToGroupAsync (Context.ConnectionId, meetingId);
        }

        public Task LeaveRoom (string meetingId) {
            return Groups.RemoveFromGroupAsync (Context.ConnectionId, meetingId);
        }
    }
}