using Microsoft.AspNetCore.Http;
using Feedback.Data_access;
using Feedback.Models;

namespace Feedback.Data.Repositories
{
    public class FeedbackRepository : Repository<Models.Feedback>, IFeedbackRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FeedbackRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}