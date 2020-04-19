using Business.Services.Interfaces;
using Data.Contexts.Roles;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {
    [Authorize]
    [ApiController]
    [Route ("[controller]")]
    public class UsersController : ControllerBase {
        // private IUserService _userService;

        // public UsersController (IUserService userService) {
        //     _userService = userService;
        // }

        // [AllowAnonymous]
        // [HttpPost ("authenticate")]
        // public IActionResult Authenticate ([FromBody] AuthenticateDTO model) {
        //     var user = _userService.Authenticate (model.Username, model.Password);

        //     if (user == null)
        //         return BadRequest (new { message = "Username or password is incorrect" });

        //     return Ok (user);
        // }

        // [Authorize (Roles.ADMIN)]
        // [HttpGet]
        // public IActionResult GetAll () {
        //     var users = _userService.GetAll ();
        //     return Ok (users);
        // }

        // [Authorize (Roles.ADMIN)]
        // [HttpGet ("{id}")]
        // public IActionResult GetById (int id) {
        //     // only allow admins to access other user records
        //     var currentUserId = int.Parse (User.Identity.Name);
        //     if (id != currentUserId && !User.IsInRole (Roles.ADMIN))
        //         return Forbid ();

        //     var user = _userService.GetById (id);

        //     if (user == null)
        //         return NotFound ();

        //     return Ok (user);
        // }
    }
}