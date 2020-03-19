using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Feedback.Application.Services.Interfaces;
using Feedback.Data.Roles;
using Feedback.Data_access.viewModels;
using Feedback.QueryParams;
using Feedback.Services;
using Feedback.Services.Interface;
using Feedback.Utils;
using Feedback.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers {
    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class MeetingController : ControllerBase {
        private readonly IMeetingService _service;
        private readonly IQuestionSetService _questionSetService;

        public MeetingController (IMeetingService service, IQuestionSetService questionSetService) {
            _service = service;
            _questionSetService = questionSetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeeting ([FromRoute] string id) {
            return Ok (await _service.GetMeeting (MeetingIdHelper.GetId (id)));
        }

        [HttpGet]
        [Route ("ShortId/{id}")]
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
        public async Task<ActionResult<IEnumerable<MeetingDTO>>> GetMeetingsByDate ([FromQuery] DateTime start, [FromQuery] DateTime end) {
            // Admin
            if (User.IsInRole (Roles.VADMIN) && !User.IsInRole (Roles.ADMIN)) {
                var userIdentity = (ClaimsIdentity) User.Identity;
                var claims = userIdentity.Claims;
            }

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
        [HttpGet]
        [Route ("MeetingOpen/{id}")]
        public async Task<IActionResult> IsMeetingOpen ([FromRoute] string id) {
            var meeting = await _service.GetMeeting (id);
            if (meeting != null) {
                var set = await _questionSetService.GetQuestionSet (meeting.QuestionsSetId);
                return Ok (set);
            }
            return NotFound ();
        }

        [HttpPost]
        [Route ("Create")]
        public void CreateMeeting ([FromBody] MeetingDTO meeting) {
            _service.CreateMeeting (meeting);
        }

        [HttpPut]
        public void UpdateMeeting ([FromBody] MeetingDTO meeting) {
            _service.UpdateMeeting (meeting);
        }

        [HttpDelete]
        [Route ("Delete")]
        public void DeleteMeeting (MeetingDTO meeting) {
            Console.WriteLine (meeting);
            _service.DeleteMeeting (meeting);
        }

        [HttpGet]
        [Route ("Categories/{CompanyId}")]
        public async Task<IEnumerable<CategoryDTO>> GetMeetingCategories ([FromRoute] int CompanyId) {
            return await _service.GetMeetingCategories (CompanyId);
        }

    }
}