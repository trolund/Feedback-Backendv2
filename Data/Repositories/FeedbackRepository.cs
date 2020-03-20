using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories {
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FeedbackRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base (context) {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}