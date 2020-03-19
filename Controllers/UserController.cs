using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feedback.Data.Roles;
using Feedback.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers
{

    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, VAdmin")]
        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var qury = _userManager.Users;

            if (!User.IsInRole(Roles.ADMIN))
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Email == User.Identity.Name);
                qury.Where(u => u.Company == user.Company);
            }
            return qury.ToList();
        }

        [Authorize(Roles = "Admin, VAdmin")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IEnumerable<ApplicationUser>> DeleteUsers(IEnumerable<ApplicationUser> usersToDelete)
        {
            var userDeleted = new List<ApplicationUser>();
            var qury = usersToDelete.AsQueryable();

            if (!User.IsInRole(Roles.ADMIN))
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Email == User.Identity.Name);
                qury.Where(u => u.Company == user.Company);
            }

            foreach (ApplicationUser user in qury.ToList())
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    userDeleted.Append(user);
                }
            }
            return userDeleted;
        }

    }
}