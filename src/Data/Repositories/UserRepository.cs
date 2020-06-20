using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class UserRepository : Repository<ApplicationUser, Guid>, IUserRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private ApplicationDbContext _context {
            get { return _context as ApplicationDbContext; }
        }

        public ApplicationUser findUsersByUsername (string username) {
            return _context.Users.SingleOrDefault (i => i.UserName == username);
        }

        public async Task<ApplicationUser> getUserByEmailwithCompany (string userId) {
            return await _context.Users
                .Include (u => u.Company)
                .SingleOrDefaultAsync (i => i.Id.Equals (userId));
        }

    }
}