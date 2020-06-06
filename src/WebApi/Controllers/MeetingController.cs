using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Data.Contexts.Roles;
using Infrastructure.QueryParams;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers {
    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class MeetingController : ControllerBase {
        private readonly IMeetingService _service;
        private readonly IQuestionSetService _questionSetService;

        private readonly IFeedbackBatchService _feedbackBatchService;

        private readonly ILogger<MeetingController> _logger;

        public MeetingController (IMeetingService service, IQuestionSetService questionSetService, IFeedbackBatchService feedbackBatchService, ILogger<MeetingController> logger) {
            _service = service;
            _questionSetService = questionSetService;
            _feedbackBatchService = feedbackBatchService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize (Policy = "activeUser")]
        public async Task<IActionResult> GetMeeting ([FromRoute] string id) {
            return Ok (await _service.GetMeeting (MeetingIdHelper.GetId (id)));
        }

        [HttpGet]
        [Route ("ShortId/{id}")]
        [Authorize (Policy = "activeUser")]
        public async Task<IActionResult> GetMeetingByShotId (string id) {
            return Ok (await _service.GetMeeting (id));
        }

        [HttpGet]
        [Route ("Pageing")]
        public ActionResult<IEnumerable<MeetingDTO>> GetMeetings ([FromQuery] MeetingResourceParameters parameters) {
            var accessParameters = new MeetingAccessParameters ();

            // Admin
            if (User.IsInRole (Roles.VADMIN) && !User.IsInRole (Roles.ADMIN)) {
                var userIdentity = (ClaimsIdentity) User.Identity;
                var claims = userIdentity.Claims;
            }

            return Ok (_service.GetMeetings (parameters));
        }

        [HttpGet]
        [Route ("ByDate")]
        [Authorize (Policy = "activeUser")]
        public async Task<ActionResult<IEnumerable<MeetingDTO>>> GetMeetingsByDate ([FromQuery] DateTime start, [FromQuery] DateTime end) {
            // Admin
            // if (User.IsInRole (Roles.VADMIN) && !User.IsInRole (Roles.ADMIN)) {
            //     var userIdentity = (ClaimsIdentity) User.Identity.Name;
            //     var claims = userIdentity.Claims;
            // }

            return Ok (await _service.GetMeetings (new MeetingDateResourceParameters () {
                Start = start,
                    End = end
            }));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route ("QrCode/{shortCodeId}")]
        public IActionResult GetQRCode ([FromRoute] string shortCodeId) {
            return Ok (File (_service.GetQRCode (shortCodeId), "image/jpeg")); //Return as file result
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("MeetingOpen/{id}")]
        public async Task<IActionResult> IsMeetingOpen ([FromRoute] string id, [FromBody] string fingerprint) {
            try {
                if (!await _service.IsMeetingOpenForFeedback (id)) {
                    return BadRequest (new { msg = "Feedback is no longer open for this meeting." });
                }

                if (await _feedbackBatchService.HaveAlreadyGivenFeedback (id, fingerprint)) {
                    return Unauthorized (new { msg = "You can only give feedback once." });
                }
            } catch (Exception e) {
                return NotFound (new { msg = "The meeting with id " + id + " was not found." });
            }

            var meeting = await _service.GetMeeting (id);
            if (meeting != null) {
                var set = await _questionSetService.GetQuestionSet (meeting.QuestionsSetId, false);
                return Ok (set);
            }
            return NotFound (new { msg = "The meeting with id " + id + " was not found." });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route ("isMeetingOpen/{id}")]
        public async Task<IActionResult> MeetingOpen ([FromRoute] string id) {
            if (!await _service.IsMeetingOpenForFeedback (id)) {
                return BadRequest (new { msg = "Feedback is no longer open for this meeting." });
            }

            var meeting = await _service.GetMeeting (id);
            if (meeting != null) {
                return Ok (new { msg = "Meeting is ready for feedback." });
            }
            return NotFound (new { msg = "The meeting with id " + id + " was not found." });
        }

        [HttpPost]
        [Route ("Create")]
        [Authorize (Policy = "activeUser")]
        public async Task<IActionResult> CreateMeeting ([FromBody] MeetingDTO meeting) {
            try {
                await _service.CreateMeeting (meeting);
                return Ok ();
            } catch (Exception e) {
                _logger.LogWarning ("meeting failed to be created " + meeting.Name, meeting, e);
                return BadRequest (e);
            }

        }

        [HttpPut]
        [Authorize (Policy = "activeUser")]
        public async Task UpdateMeeting ([FromBody] MeetingDTO meeting) {
            await _service.UpdateMeeting (meeting);
        }

        [HttpDelete]
        [Route ("Delete")]
        [Authorize (Policy = "activeUser")]
        public void DeleteMeeting (MeetingDTO meeting) {
            Console.WriteLine (meeting);
            _service.DeleteMeeting (meeting);
        }

        [HttpGet]
        [Route ("Categories/{CompanyId}")]
        [Authorize (Policy = "activeUser")]
        public async Task<IEnumerable<CategoryDTO>> GetMeetingCategories ([FromRoute] int CompanyId) {
            return await _service.GetMeetingCategories (CompanyId);
        }

        [HttpGet]
        [Route ("ByDay/{date}")]
        [Authorize (Policy = "activeUser")]
        public async Task<IEnumerable<MeetingDTO>> GetMeetingsOneDay ([FromRoute] DateTime date) {
            return await _service.GetMeetingsOneDay (date);
        }

    }
}