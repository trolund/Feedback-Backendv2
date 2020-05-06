using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Controllers {
    [Authorize]
    [ApiController]

    [Route ("Api/[controller]")]
    public class FeedbackBatchController : ControllerBase, IBaseController<FeedbackBatchDTO, string> {

        private readonly IFeedbackBatchService _service;

        private IHubContext<LiveFeedbackHub> _hub;

        public IFeedbackBatchService Service => _service;

        public IMeetingService _meetingService;

        public FeedbackBatchController (IFeedbackBatchService service, IHubContext<LiveFeedbackHub> hub, IMeetingService meetingService) {
            _service = service;
            _hub = hub;
            _meetingService = meetingService;
        }

        [Authorize (Roles = "Admin")]
        [HttpDelete]
        public void Delete (FeedbackBatchDTO entity) {
            Service.Delete (entity);
        }

        [HttpGet]
        [Route ("{meetingId}")]
        public async Task<IActionResult> GetAll ([FromRoute] string meetingId) {
            return Ok (await Service.GetAllFeedbackBatchByMeetingId (meetingId));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ObtainFeedback ([FromBody] FeedbackBatchDTO entity) {

            if (await Service.HaveAlreadyGivenFeedback (entity.MeetingId, entity.UserFingerprint)) {
                return BadRequest (new { msg = "You can only give feedback once." });
            }

            if (await _meetingService.IsMeetingOpenForFeedback (entity.MeetingId)) {
                return BadRequest (new { msg = "Feedback is no longer open for this meeting." });
            }

            if (await Service.Create (entity)) {
                // send feedback to all real-time feedback observers
                _hub.Clients.Group (entity.MeetingId).SendAsync ("sendfeedback", await _service.GetAllFeedbackBatchByMeetingId (entity.MeetingId));
                return Ok ();
            } else {
                return BadRequest (new { msg = "Feedback have not been delivered." });
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public void Put ([FromBody] FeedbackBatchDTO entity) {
            Service.Create (entity);
        }

        void IBaseController<FeedbackBatchDTO, string>.Delete (FeedbackBatchDTO entity) {
            throw new NotImplementedException ();
        }

        FeedbackBatchDTO IBaseController<FeedbackBatchDTO, string>.Get (string id) {
            throw new NotImplementedException ();
        }

        IEnumerable<FeedbackBatchDTO> IBaseController<FeedbackBatchDTO, string>.GetAll () {
            throw new NotImplementedException ();
        }

        void IBaseController<FeedbackBatchDTO, string>.Post (FeedbackBatchDTO entity) {
            throw new NotImplementedException ();
        }

        void IBaseController<FeedbackBatchDTO, string>.Put (FeedbackBatchDTO entity) {
            throw new NotImplementedException ();
        }

        [HttpGet]
        [Authorize (Roles = "Admin, VAdmin, Facilitator")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("dashboard")]
        public async Task<IActionResult> Dashboard ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string[] categories, [FromQuery] string searchWord) {

            var result = await Service.OwnFeedback (start, end, categories, searchWord);
            var list = result.SelectMany (i => i.Feedback).Select (i => new { Anwser = i.Answer, Comment = i.Comment });
            return Ok (list);
        }

        [HttpGet]
        [Authorize (Roles = "Admin, VAdmin, Facilitator")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("dashboardMonth")]
        public async Task<IActionResult> DashboardMonth ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string[] categories, [FromQuery] string searchWord, [FromQuery] bool onlyOwnData) {
            return Ok (await Service.OwnFeedbackMonth (start, end, categories, searchWord, onlyOwnData));
        }

        [HttpGet]
        [Authorize (Roles = "Admin, VAdmin, Facilitator")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("dashboardDate")]
        public async Task<IActionResult> DashboardDate ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string[] categories, [FromQuery] string searchWord, [FromQuery] bool onlyOwnData) {
            return Ok (await Service.OwnFeedbackDate (start, end, categories, searchWord, onlyOwnData));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("rating")]
        public async Task<IActionResult> getRating () {
            return Ok (await Service.GetUserRating ());
        }
    }

}