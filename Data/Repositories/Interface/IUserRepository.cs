using System;
using Data.Models;

namespace Data.Repositories.Interface {
    public interface IUserRepository : IRepository<ApplicationUser, Guid> {
        ApplicationUser findUsersByUsername (string username);
    }
}