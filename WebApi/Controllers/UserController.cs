﻿using System.Collections.Generic;
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
                return BadRequest (new { message = "Username or password is incorrect" });

            return Ok (user);
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [HttpGet]
        [Route ("all")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers () {
            var qury = _userManager.Users;

            if (!User.IsInRole (Roles.ADMIN)) {
                var user = _userManager.Users.SingleOrDefault (u => u.Email == User.Identity.Name);
                qury.Where (u => u.Company == user.Company);
            }
            return qury.ToList ();
        }

        [Authorize (Roles = "Admin, VAdmin")]
        [HttpDelete]
        [Route ("delete")]
        public async Task<IEnumerable<ApplicationUser>> DeleteUsers (IEnumerable<ApplicationUser> usersToDelete) {
            var userDeleted = new List<ApplicationUser> ();
            var qury = usersToDelete.AsQueryable ();

            if (!User.IsInRole (Roles.ADMIN)) {
                var user = _userManager.Users.SingleOrDefault (u => u.Email == User.Identity.Name);
                qury.Where (u => u.Company == user.Company);
            }

            foreach (ApplicationUser user in qury.ToList ()) {
                var result = await _userManager.DeleteAsync (user);
                if (result.Succeeded) {
                    userDeleted.Append (user);
                }
            }
            return userDeleted;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("Post")]
        public async Task<ActionResult<UserDTO>> UserRegistration (UserRegistrationDTO Entity) {
            var companyConfirmed = false;
            // if company shoud be created
            if (Entity.company != null) {
                Entity.CompanyId = await _companyService.CreateCompany (Entity.company);
                companyConfirmed = true;
                Entity.RequesetedRoles.Append (Roles.VADMIN);
            }

            var user = new ApplicationUser () {
                CompanyId = Entity.CompanyId,
                CompanyConfirmed = companyConfirmed,
                Email = Entity.Email,
                UserName = Entity.Email,
                Lastname = Entity.Lastname,
                Firstname = Entity.Firstname,
                EmailConfirmed = false,
                PhoneNumber = Entity.Phone,
            };

            await _userManager.CreateAsync (user, Entity.Password);
            var newUser = await _userManager.FindByEmailAsync (Entity.Email);
            await _userManager.AddToRolesAsync (newUser, Entity.RequesetedRoles);

            return _mapper.Map<UserDTO> (user);
        }

    }
}