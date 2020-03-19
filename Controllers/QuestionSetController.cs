using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feedback.Application.Services.Interfaces;
using Feedback.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers {
    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class QuestionSetController : ControllerBase {
        private readonly IQuestionSetService _service;
        public QuestionSetController (IQuestionSetService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get () {
            return Ok (await _service.GetQuestionSets ());
        }

        // GET api/values/5
        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return _service.getQuestionSet(id);
        // }
        [HttpGet]
        [Route ("names")]
        public async Task<IActionResult> GetAllQuestionSetNames () {
            return Ok (await _service.GetQuestionSetNames ());
        }

        [HttpGet]
        [Route ("{questionId}")]
        public async Task<ActionResult<QuestionSetDTO>> GetQuestionSetById (string questionId) {
            return Ok (await _service.GetQuestionSet (questionId));
        }

        [HttpGet]
        [Route ("SetOnly")]
        public async Task<ActionResult<IEnumerable<QuestionSetDTO>>> GetAllQuestionSet () {
            return Ok (await _service.GetQuestionSetOnly ());
        }

        // POST api/values
        [HttpPost]
        public void Post ([FromBody] QuestionSetDTO value) {
            _service.UpdateQuestionSet (value);
        }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] QuestionSetDTO value) {
            _service.CreateQuestionSet (value);
        }

        // DELETE api/values/5
        [HttpDelete ("{id}")]
        public void Delete (QuestionSetDTO value) {
            _service.DeleteQuestionSet (value);
        }

    }
}