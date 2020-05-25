using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Business.Services {
    public class CompanyService : ICompanyService {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateCompany (CompanyDTO Company) {
            var newCompany = _mapper.Map<Company> (Company);
            await _unitOfWork.Company.Add (newCompany);
            await _unitOfWork.SaveAsync ();
            return newCompany.CompanyId;
        }

        public void DeleteCompany (CompanyDTO Company) {
            _unitOfWork.Company.Remove (_mapper.Map<Company> (Company));
        }

        public CompanyDTO getCompany (int id) {
            return _mapper.Map<CompanyDTO> (_unitOfWork.Company.Find (c => c.CompanyId == id));
        }

        public ApplicationUser getCompanyAdmin (string CompanyId) {
            return _unitOfWork.Company.getCompanyAdmin (int.Parse (CompanyId));
        }

        public ApplicationUser getCompanyAdmin (int CompanyId) {
            return _unitOfWork.Company.getCompanyAdmin (CompanyId);
        }

        public IEnumerable<CompanyDTO> GetCompanys () {
            return _mapper.Map<IEnumerable<CompanyDTO>> (_unitOfWork.Company.GetAll ());
        }

        public CompanyDTO UpdateCompany (CompanyDTO Company) {
            _unitOfWork.Company.Add (_mapper.Map<Company> (Company));
            return Company;
        }
    }
}