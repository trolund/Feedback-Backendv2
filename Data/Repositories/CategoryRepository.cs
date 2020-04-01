using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;
        public CategoryRepository (ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base (context) {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<List<Category>> getAllCategoriesForMeeting (List<Guid> Ids) {
            var collection = _context.Categories as IQueryable<Category>;

            foreach (Guid id in Ids) {
                collection.Where (item => item.CategoryId == id);
            }

            return await collection.ToListAsync ();
        }
    }
}