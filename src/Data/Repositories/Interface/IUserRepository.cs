using System;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repositories.Interface {
    public interface IUserRepository : IRepository<ApplicationUser, Guid> {
        ApplicationUser findUsersByUsername (string username);

        Task<ApplicationUser> getUserByEmailwithCompany (string userId);
    }
}