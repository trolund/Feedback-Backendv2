using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace Business.Services {
    public class CategoriesService {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryRepository _service;

        public CategoriesService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Category>> getAllCategories () {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            return await _service.getAllCategories (Int32.Parse (companyId));
        }

        public async void createCategory (Category entity) {
            _service.createCategory (entity);
            await _unitOfWork.SaveAsync ();
        }

        public async void updateCategory (Category entity) {
            _service.updateCategory (entity);
            await _unitOfWork.SaveAsync ();
        }

        public async void deleteCategory (Guid CategoryId) {
            _service.deleteCategory (CategoryId);
            await _unitOfWork.SaveAsync ();
        }
    }
}