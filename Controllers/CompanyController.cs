using Feedback.Services.Interface;
using Feedback.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class CompanyController : ControllerBase, IBaseController<CompanyDTO, int>
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpDelete]
        public void Delete(CompanyDTO entity)
        {
            _service.CreateCompany(entity);
        }

        [HttpGet]
        public CompanyDTO Get(int id)
        {
            return _service.getCompany(id);
        }

        [HttpGet]
        [Route("all")]
        public System.Collections.Generic.IEnumerable<CompanyDTO> GetAll()
        {
            return _service.GetCompanys();
        }

        [HttpPost]
        public void Post([FromBody] CompanyDTO entity)
        {
            _service.UpdateCompany(entity);
        }

        [HttpPut]
        public void Put([FromBody] CompanyDTO entity)
        {
            _service.CreateCompany(entity);
        }
    }
}