using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories {
    public class CompanyRepository : Repository<Company>, ICompanyRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base (context) {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}