using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Business.Helpers;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService (IOptions<AppSettings> appSettings, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = _unitOfWork = new UnitOfWork (context, httpContextAccessor, mapper);
            _signInManager = signInManager;
        }

        public async Task<UserDTO> Authenticate (LoginDTO loginDTO) {
            var user = await _unitOfWork.Users.SingleOrDefault (user => user.UserName == loginDTO.Email);
            var roles = await _userManager.GetRolesAsync (user);
            var result = await _signInManager.PasswordSignInAsync (loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure : false);

            // authentication successful so generate jwt token
            if (user != null && result.Succeeded) {
                var userDTO = _mapper.Map<UserDTO> (user);

                var tokenHandler = new JwtSecurityTokenHandler ();
                var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity (new Claim[] {
                    new Claim (ClaimTypes.Name, user.UserName),
                    new Claim (ClaimTypes.Role, JsonSerializer.Serialize (roles))
                    }),
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

    }
}