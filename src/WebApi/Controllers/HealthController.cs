using Business.Services.Interfaces;
using Data.Contexts.Roles;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {

    [ApiController]
    [Route ("[controller]")]
    public class HealthController : ControllerBase {
        [HttpGet]
        public string Get (int id) {
            return "Service is running and is Healthy! :)";
        }
    }
}