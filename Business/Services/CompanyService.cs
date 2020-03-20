using System.Collections.Generic;
using AutoMapper;
using Feedback.Data;
using Feedback.Services.Interface;
using Feedback.viewModels;
using Microsoft.AspNetCore.Http;
using Feedback.Models;

namespace Feedback.Application.Services
{
    public class CompanyService : ICompanyService
    {

        private UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public void CreateCompany(CompanyDTO Company)
        {
            _unitOfWork.Company.Add(_mapper.Map<Company>(Company));
        }

        public void DeleteCompany(CompanyDTO Company)
        {
            _unitOfWork.Company.Remove(_mapper.Map<Company>(Company));
        }

        public CompanyDTO getCompany(int id)
        {
            return _mapper.Map<CompanyDTO>(_unitOfWork.Company.Find(c => c.CompanyId == id));
        }

        public IEnumerable<CompanyDTO> GetCompanys()
        {
            return _mapper.Map<IEnumerable<CompanyDTO>>(_unitOfWork.Company.GetAll());
        }

        public CompanyDTO UpdateCompany(CompanyDTO Company)
        {
            _unitOfWork.Company.Add(_mapper.Map<Company>(Company));
            return Company;
        }
    }
}