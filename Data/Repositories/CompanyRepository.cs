using Feedback.Data;
using Feedback.Data.Repositories;
using Feedback.Data_access.Repositories.Interfaces;
using Feedback.Models;
using Microsoft.AspNetCore.Http;

namespace Feedback.Data_access.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}