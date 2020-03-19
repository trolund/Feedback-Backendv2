using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feedback.Data.Roles;
using Feedback.Data_access.viewModels;
using Feedback.Models;
using Feedback.Services.Interface;
using Feedback.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers {
    [Authorize]
    [ApiController]

    [Route ("Api/[controller]")]
    public class FeedbackBatchController : ControllerBase, IBaseController<FeedbackBatchDTO, string> {

        private readonly IFeedbackBatchService _service;

        public FeedbackBatchController (IFeedbackBatchService service) {
            _service = service;
        }

        [HttpDelete]
        public void Delete (FeedbackBatchDTO entity) {
            _service.Delete (entity);
        }

        [HttpGet]
        [Route ("{meetingId}")]
        public async Task<IActionResult> GetAll ([FromRoute] string meetingId) {
            return Ok (await _service.GetAllFeedbackBatchByMeetingId (meetingId));
        }

        [AllowAnonymous]
        [HttpPost]
        public void Post ([FromBody] FeedbackBatchDTO entity) {
            _service.Create (entity);
        }

        [AllowAnonymous]
        [HttpPut]
        public void Put ([FromBody] FeedbackBatchDTO entity) {
            _service.Create (entity);
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
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("dashboard")]
        public async Task<IActionResult> Dashboard ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string[] categories, [FromQuery] string searchWord) {

            var result = await _service.OwnFeedback (start, end, categories, searchWord);
            var list = result.SelectMany (i => i.Feedback).Select (i => new { Anwser = i.Answer, Comment = i.Comment });
            return Ok (list);
        }

        [HttpGet]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [Route ("dashboardMonth")]
        public async Task<IActionResult> DashboardMonth ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string[] categories, [FromQuery] string searchWord, [FromQuery] bool onlyOwnData) {
            return Ok (await _service.OwnFeedbackMonth (start, end, categories, searchWord, onlyOwnData));
        }
    }
}