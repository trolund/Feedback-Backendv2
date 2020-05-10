using Data.Models;

namespace Data.Repositories.Interface {
    public interface ICompanyRepository : IRepository<Company, int> {

        ApplicationUser getCompanyAdmin (int CompanyId);

    }
}