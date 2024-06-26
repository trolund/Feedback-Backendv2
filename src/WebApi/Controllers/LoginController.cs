using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers {

    [AllowAnonymous]
    public class LoginController : ControllerBase {

        //     private readonly UserManager<ApplicationUser> _userManager;
        //     private readonly SignInManager<ApplicationUser> _signInManager;
        //     private readonly ILogger<LoginController> _logger;

        //     public LoginController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LoginController> logger) {
        //         _userManager = userManager;
        //         _signInManager = signInManager;
        //         _logger = logger;
        //     }

        //     public IList<AuthenticationScheme> ExternalLogins { get; set; }

        //     public string ReturnUrl { get; set; }

        //     [TempData]
        //     public string ErrorMessage { get; set; }

        //     public async Task OnGetAsync (string returnUrl = null) {
        //         if (!string.IsNullOrEmpty (ErrorMessage)) {
        //             ModelState.AddModelError (string.Empty, ErrorMessage);
        //         }

        //         returnUrl = returnUrl ?? Url.Content ("~/");

        //         // Clear the existing external cookie to ensure a clean login process
        //         await HttpContext.SignOutAsync (IdentityConstants.ExternalScheme);

        //         ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync ()).ToList ();

        //         ReturnUrl = returnUrl;
        //     }

        //     [HttpPost]
        //     [Route ("login")]
        //     public async Task<IActionResult> Login ([FromBody] LoginDTO Input, string returnUrl = null) {
        //         returnUrl = returnUrl ?? Url.Content ("~/");

        //         if (ModelState.IsValid) {
        //             // This doesn't count login failures towards account lockout
        //             // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //             var result = await _signInManager.PasswordSignInAsync (Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure : false);
        //             if (result.Succeeded) {
        //                 _logger.LogInformation ("User logged in.");
        //                 return LocalRedirect (returnUrl);
        //             }
        //             if (result.RequiresTwoFactor) {
        //                 return RedirectToPage ("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        //             }
        //             if (result.IsLockedOut) {
        //                 _logger.LogWarning ("User account locked out.");
        //                 return RedirectToPage ("./Lockout");
        //             } else {
        //                 ModelState.AddModelError (string.Empty, "Invalid login attempt.");
        //                 _logger.LogWarning ("Invalid login attempt.");
        //                 return Ok ();
        //             }
        //         }

        //         // If we got this far, something failed, redisplay form
        //         return Ok ();
        //     }

        // }
    }
}