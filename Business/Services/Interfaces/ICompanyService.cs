using System.Collections.Generic;
using Feedback.viewModels;

namespace Feedback.Services.Interface
{
    public interface ICompanyService
    {
        CompanyDTO getCompany(int id);

        IEnumerable<CompanyDTO> GetCompanys();

        void CreateCompany(CompanyDTO Company);

        CompanyDTO UpdateCompany(CompanyDTO Company);

        void DeleteCompany(CompanyDTO Company);
    }
}