using System.Threading.Tasks;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {
    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class CategoriesController : ControllerBase {
        private readonly ICompanyService _service;

        public CategoriesController (ICompanyService service) {
            _service = service;
        }

        [HttpDelete]
        [Authorize (Policy = "activeUser")]
        public void Delete (CompanyDTO entity) {
            _service.CreateCompany (entity);
        }

        [HttpGet]
        public async Task<CompanyDTO> Get (int id) {
            return await _service.getCompany (id);
        }

        [HttpGet]
        [Route ("all")]
        public System.Collections.Generic.IEnumerable<CompanyDTO> GetAll () {
            return _service.GetCompanys ();
        }

        [HttpPost]
        public void Post ([FromBody] CompanyDTO entity) {
            _service.UpdateCompany (entity);
        }

        [HttpPut]
        public void Put ([FromBody] CompanyDTO entity) {
            _service.CreateCompany (entity);
        }
    }
}