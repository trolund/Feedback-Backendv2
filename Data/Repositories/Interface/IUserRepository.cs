using Data.Models;

namespace Data.Repositories.Interface {
    public interface IUserRepository : IRepository<ApplicationUser> {
        ApplicationUser findUsersByUsername (string username);
    }
}