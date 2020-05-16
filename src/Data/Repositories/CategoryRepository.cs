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
                collection = collection.Where (item => item.CategoryId == id);
            }

            collection.Where (c => c.active == true);

            return await collection.ToListAsync ();
        }

        public async Task<List<Category>> getAllCategories (int companyId) {
            var collection = _context.Categories as IQueryable<Category>;

            collection = collection.Where (c => c.active == true);
            collection = collection.Where (c => c.CompanyId.Equals (companyId) ||  c.CompanyId.Equals (Environment.GetEnvironmentVariable ("SpinOffCompanyID")));

            return await collection.ToListAsync ();
        }

        public void createCategory (Category entity) {
            _context.Categories.Add (entity);
        }

        public void updateCategory (Category entity) {
            _context.Categories.Update (entity);
        }

        public async void deleteCategory (Guid CategoryId) {
            var cateToUpdate = await _context.Categories.Where (c => c.CategoryId.Equals (CategoryId)).FirstAsync ();
            cateToUpdate.active = false;
        }
    }
}