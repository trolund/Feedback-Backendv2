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

namespace WebApi.Controllers {
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
        public async Task<IActionResult> CreateMeeting ([FromBody] MeetingDTO meeting) {
            await _service.CreateMeeting (meeting);
            return Ok ();
        }

        [HttpPut]
        public async Task UpdateMeeting ([FromBody] MeetingDTO meeting) {
            await _service.UpdateMeeting (meeting);
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