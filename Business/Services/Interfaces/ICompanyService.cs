using System.Collections.Generic;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface ICompanyService {
        CompanyDTO getCompany (int id);

        IEnumerable<CompanyDTO> GetCompanys ();

        void CreateCompany (CompanyDTO Company);

        CompanyDTO UpdateCompany (CompanyDTO Company);

        void DeleteCompany (CompanyDTO Company);
    }
}