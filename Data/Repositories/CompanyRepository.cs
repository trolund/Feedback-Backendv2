using Data.Contexts;
using Data.Contexts.Repositories;
using Data.Contexts_access.Repositories.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Data.Contexts_access.Repositories {
    public class CompanyRepository : Repository<Company>, ICompanyRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base (context) {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}