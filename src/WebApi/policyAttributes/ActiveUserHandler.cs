using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.ApiRequirement {
    public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement> {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ActiveUserHandler (IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync (
            AuthorizationHandlerContext authContext, ActiveUserRequirement requirement) {
            var httpContext = httpContextAccessor.HttpContext;
            var companyConfirmed = bool.Parse (httpContext.User.Claims.Where (c => c.Type == "CCON").First ().Value);

            if (companyConfirmed) {
                authContext.Succeed (requirement);
            } else {
                authContext.Fail ();
            }

            return Task.CompletedTask;
        }
    }
}