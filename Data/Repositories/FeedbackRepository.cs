using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Data.Contexts.Repositories {
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FeedbackRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base (context) {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}