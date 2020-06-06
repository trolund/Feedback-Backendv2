using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {
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

        [HttpGet]
        [Route ("names")]
        [Authorize (Policy = "activeUser")]
        public async Task<IActionResult> GetAllQuestionSetNames () {
            return Ok (await _service.GetQuestionSetNames ());
        }

        [HttpGet]
        [Route ("{questionId}")]
        [Authorize (Policy = "activeUser")]
        public async Task<ActionResult<QuestionSetDTO>> GetQuestionSetById (string questionId) {
            return Ok (await _service.GetQuestionSet (questionId, false));
        }

        [HttpGet]
        [Route ("SetOnly")]
        [Authorize (Policy = "activeUser")]
        public async Task<ActionResult<IEnumerable<QuestionSetDTO>>> GetAllQuestionSet () {
            return Ok (await _service.GetQuestionSetOnly ());
        }

        [HttpPost]
        [Authorize (Roles = "Admin, VAdmin")]
        [Authorize (Policy = "activeUser")]
        public async Task<IActionResult> Post ([FromBody] QuestionSetDTO value) {
            if (await _service.UpdateQuestionSet (value)) {
                return Ok ();
            }
            return BadRequest ();
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [Authorize (Policy = "activeUser")]
        [HttpPut]
        public async Task<IActionResult> Put (int id, [FromBody] QuestionSetDTO value) {
            if (await _service.CreateQuestionSet (value)) {
                return Ok ();
            }
            return BadRequest ();
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [Authorize (Policy = "activeUser")]
        [HttpDelete]
        public async Task<IActionResult> Delete (QuestionSetDTO value) {
            try {
                if (await _service.DeleteQuestionSet (value)) {
                    return Ok ();
                } else {
                    throw new Exception ("Question set faild to be deleted.");
                }
            } catch (Exception e) {
                return BadRequest ();
            }
        }

    }
}