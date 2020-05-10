using System.Linq;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories {
    public class CompanyRepository : Repository<Company, int>, ICompanyRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base (context) {
            _httpContextAccessor = httpContextAccessor;
        }

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public ApplicationUser getCompanyAdmin (int CompanyId) {
            var collection = _context.Users as IQueryable<ApplicationUser>;
            collection = collection.Where (u => u.CompanyId.Equals (CompanyId));
            return collection.First ();
        }

    }
}