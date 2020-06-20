using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {
    [Authorize]
    [ApiController]
    [Route ("Api/[controller]")]
    public class CompanyController : ControllerBase {
        private readonly ICompanyService _service;

        public CompanyController (ICompanyService service) {
            _service = service;
        }

        [HttpDelete]
        public void Delete (CompanyDTO entity) {
            _service.CreateCompany (entity);
        }

        [HttpGet]
        public async Task<CompanyDTO> Get (int id) {
            return await _service.getCompany (id);
        }

        [HttpGet]
        [Route ("all")]
        public IEnumerable<CompanyDTO> GetAll () {
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