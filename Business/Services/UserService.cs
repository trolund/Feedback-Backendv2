using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Business.Helpers;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services {

    public class UserService : IUserService {
        // UserDTOs hardcoded for simplicity, store in a db with hashed passwords in production applications
        // private List<UserDTO> _UserDTOs = new List<UserDTO> {
        //     new UserDTO { Id = 1, FirstName = "Admin", LastName = "UserDTO", UserDTOname = "admin", Password = "admin", Role = Roles.ADMIN },
        //     new UserDTO { Id = 2, FirstName = "Normal", LastName = "UserDTO", UserDTOname = "UserDTO", Password = "UserDTO", Role = Roles.FACILITATOR }
        // };
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public UserService (IOptions<AppSettings> appSettings, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IEmailService emailService, ICompanyService companyService, IUrlHelper urlHelper) {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        public async Task<UserDTO> Authenticate (LoginDTO loginDTO) {
            var user = await _unitOfWork.Users.SingleOrDefault (user => user.NormalizedEmail == loginDTO.Email.Normalize (NormalizationForm.FormC));
            var roles = await _userManager.GetRolesAsync (user);
            var result = await _signInManager.PasswordSignInAsync (user.UserName, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure : false);

            // authentication successful so generate jwt token
            if (user != null && result.Succeeded) {
                var userDTO = _mapper.Map<UserDTO> (user);

                var tokenHandler = new JwtSecurityTokenHandler ();
                var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity (await GetValidClaims (user)),
                    Expires = DateTime.UtcNow.AddDays (7),
                    SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken (tokenDescriptor);
                userDTO.Token = tokenHandler.WriteToken (token);

                return userDTO;
            } else {
                return null;
            }
        }

        public async Task signout () {
            await _signInManager.SignOutAsync ();
        }

        private async Task<List<Claim>> GetValidClaims (ApplicationUser user) {
            IdentityOptions _options = new IdentityOptions ();
            var claims = new List<Claim> {
                new Claim (JwtRegisteredClaimNames.Sub, user.Id.ToString ()),
                new Claim ("CID", user.CompanyId.ToString ()),
                // new Claim ("sub", user.Id),
                // new Claim (JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator ()),
                // new Claim (JwtRegisteredClaimNames.Iat, ToUnixEpochDate (_jwtOptions.IssuedAt).ToString (), ClaimValueTypes.Integer64),
                // new Claim (_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString ()),
                // new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (_options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };
            var userClaims = await _userManager.GetClaimsAsync (user);
            var userRoles = await _userManager.GetRolesAsync (user);
            claims.AddRange (userClaims);
            foreach (var userRole in userRoles) {
                claims.Add (new Claim (ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync (userRole);
                if (role != null) {
                    var roleClaims = await _roleManager.GetClaimsAsync (role);
                    foreach (Claim roleClaim in roleClaims) {
                        claims.Add (roleClaim);
                    }
                }
            }
            return claims;
        }

        public async Task<UserDTO> UserRegistration (UserRegistrationDTO Entity) {
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

            var res = await _userManager.CreateAsync (user, Entity.Password);
            var newUser = await _userManager.FindByEmailAsync (Entity.Email);
            var userres = await _userManager.AddToRolesAsync (newUser, Entity.RequesetedRoles);

            if (res.Succeeded && userres.Succeeded) {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync (user);
                var baseUrl = Environment.GetEnvironmentVariable ("BASE_URL");

                string confirmationLink = baseUrl + "/Api/User/confirm?token=" + HttpUtility.UrlEncode (token) + "&email=" + Entity.Email;

                await _emailService.SendEmailAsync (Entity.Email, "konto confimation", "confirm link: " + confirmationLink);

                return _mapper.Map<UserDTO> (user);
            }
            return null;
        }

        public async Task<bool> ConfirmationUser (string email, string emailToken) {
            // var decodedToken = HttpUtility.UrlDecode (emailToken);
            var user = await _userManager.FindByEmailAsync (email);
            var res = await _userManager.ConfirmEmailAsync (user, emailToken);
            if (res.Succeeded) {
                return true;
            }
            return false;
        }

        // private async Task ConfirmUsersCompanyRelation (UserAdminDTO[] input) {
        //     var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN);

        //     if (isAdmin) {
        //         foreach (var item in input) {
        //             var user = await _userManager.FindByIdAsync (item.Id.ToString ());
        //             user.CompanyConfirmed = item.CompanyConfirmed;
        //             await _userManager.UpdateAsync (user);
        //         }
        //     } else {
        //         foreach (var item in input) {
        //             var user = await _userManager.FindByIdAsync (item.Id.ToString ());
        //             user.CompanyConfirmed = item.CompanyConfirmed;
        //             await _userManager.UpdateAsync (user);
        //         }
        //     }

        // }

        // this is only called by Admin and Vadmin.
        public async Task<ICollection<UserAdminDTO>> UpdateUserAdmin (IEnumerable<UserAdminDTO> usersToUpdate) {
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN);
            var companyId = _httpContextAccessor.HttpContext.User.FindFirstValue ("CID");
            var updatedUsers = new List<UserAdminDTO> ();

            foreach (var user in usersToUpdate.ToArray ()) {
                ApplicationUser appUser = await _userManager.FindByIdAsync (user.Id.ToString ());
                if (!isAdmin) {
                    if (appUser.CompanyId != Int32.Parse (companyId)) {
                        appUser = null;
                    }
                }

                if (appUser != null) {
                    IdentityResult deleteResult = null;
                    IdentityResult updateResult = null;
                    if (user.delete) {
                        deleteResult = await _userManager.DeleteAsync (appUser);
                    }
                    if (!user.CompanyConfirmed.Equals (appUser.CompanyConfirmed)) {
                        appUser.CompanyConfirmed = user.CompanyConfirmed;
                        updateResult = await _userManager.UpdateAsync (appUser);
                    }

                    if (deleteResult != null && deleteResult.Succeeded) { user.delete = true; }
                    if (updateResult != null && updateResult.Succeeded) {
                        user.CompanyConfirmed = user.CompanyConfirmed;
                    }
                    updatedUsers.Add (user);
                }

            }

            return updatedUsers;
        }

    }
}