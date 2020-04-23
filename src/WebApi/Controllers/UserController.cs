using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts.Roles;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FWebApi.Controllers {

    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class UserController : ControllerBase {

        private readonly UserManager<ApplicationUser> _userManager;

        private IUserService _userService;

        private ICompanyService _companyService;

        private readonly IMapper _mapper;

        public UserController (UserManager<ApplicationUser> userManager, IUserService userService, ICompanyService companyService, IMapper mapper) {
            _userManager = userManager;
            _userService = userService;
            _companyService = companyService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost ("authenticate")]
        public async Task<IActionResult> Authenticate ([FromBody] LoginDTO model) {
            var user = await _userService.Authenticate (model);

            if (user == null)
                return Unauthorized (new { message = "Username or password is incorrect" });

            return Ok (user);
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [HttpGet]
        [Route ("all")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers () {
            var query = _userManager.Users;

            if (!User.IsInRole (Roles.ADMIN)) {
                var user = _userManager.Users.SingleOrDefault (u => u.Email == User.Identity.Name);
                query.Where (u => u.Company == user.Company);
            }
            return query.ToList ();
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [HttpGet]
        [Route ("userAdmin")]
        public ActionResult<IEnumerable<ApplicationUser>> GetUsers ([FromQuery] string searchword, [FromQuery] int companyConfirmed = -1, [FromQuery] int pageNumber = -1) {
            var query = _userManager.Users as IQueryable<ApplicationUser>;

            if (!User.IsInRole (Roles.ADMIN)) {
                var user = _userManager.Users.SingleOrDefault (u => u.Email == User.Identity.Name);
                query = query.Where (u => u.Company == user.Company);
            }

            if (!string.IsNullOrWhiteSpace (searchword) && !searchword.Equals ("null")) {
                var searchQuery = searchword.Trim ();
                query = query.Where (u =>
                    u.Firstname.Contains (searchQuery) ||
                    u.Lastname.Contains (searchQuery) ||
                    u.PhoneNumber.Contains (searchQuery) ||
                    u.Email.Contains (searchQuery) ||
                    u.UserName.Contains (searchQuery));
            }

            if (companyConfirmed != -1) {
                bool confirm = (companyConfirmed == 1);
                query = query.Where (u => u.CompanyConfirmed.Equals (confirm));
            }

            if (!string.IsNullOrWhiteSpace (searchword) && searchword != "null" && companyConfirmed == -1 && pageNumber > 0) {
                return query.Skip ((pageNumber - 1) * 50).Take (50).ToList ();
            }

            if (!string.IsNullOrWhiteSpace (searchword) && searchword != "null" && companyConfirmed == -1) {
                return query.Take (50).ToList ();
            }

            return query.ToList ();
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [HttpPut]
        [Route ("userAdmin")]
        public async Task<ICollection<UserAdminDTO>> UpdateUserAdmin (ICollection<UserAdminDTO> usersToUpdate) {
            return await _userService.UpdateUserAdmin (usersToUpdate);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("Post")]
        public async Task<ActionResult<UserDTO>> UserRegistration ([FromBody] UserRegistrationDTO Entity) {
            var user = await _userService.UserRegistration (Entity);
            if (user != null) {
                return Ok ();
            }
            return BadRequest ();
        }

        [HttpPost]
        [Route ("signout")]
        public async Task<ActionResult> signout () {
            await _userService.signout ();
            return Ok ();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route ("confirm")]
        public async Task<ActionResult> ConfirmUser ([FromQuery] string email, [FromQuery] string token) {
            if (await _userService.ConfirmationUser (email, token)) {
                return Ok ("Your account is know active.");
            }
            return BadRequest ();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("requestPasswordReset")]
        public async Task<ActionResult> RequestPasswordReset ([FromQuery] string email) {
            await _userService.GetResetPasswordToken (email);
            return Ok ("Reset link sendt to your email.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("confirmPasswordReset")]
        public async Task<ActionResult> ConfirmPasswordReset ([FromBody] NewPasswordDTO input) {
            if (await _userService.ResetPassword (input.Email, input.Token, input.NewPassword, input.NewPasswordAgain)) {
                return Ok ("Reset link sendt to your email.");
            }
            return BadRequest ("Request faild");
        }

        [HttpPost]
        [Route ("newPassword")]
        public async Task<ActionResult> NewPassword ([FromBody] NewPasswordDTO input) {
            if (await _userService.NewPassword (input)) {
                return Ok ("New password confirmed.");
            }
            return BadRequest ("Request faild");
        }

    }
}