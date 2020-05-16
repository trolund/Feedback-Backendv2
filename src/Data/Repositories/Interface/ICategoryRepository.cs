using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Infrastructure.ViewModels;

namespace Data.Repositories.Interface {
    public interface ICategoryRepository : IRepository<Category, Guid> {

        Task<List<Category>> getAllCategoriesForMeeting (List<Guid> Ids);
        Task<List<Category>> getAllCategories (int companyId);
        void createCategory (Category entity);
        void updateCategory (Category entity);
        void deleteCategory (Guid CategoryId);
    }
}