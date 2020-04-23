using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface ICompanyService {
        CompanyDTO getCompany (int id);

        IEnumerable<CompanyDTO> GetCompanys ();

        Task<int> CreateCompany (CompanyDTO Company);

        CompanyDTO UpdateCompany (CompanyDTO Company);

        void DeleteCompany (CompanyDTO Company);
    }
}