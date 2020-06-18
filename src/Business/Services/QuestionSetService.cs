using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Business.Services {
    public class QuestionSetService : IQuestionSetService {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionSetService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<QuestionSetDTO> GetQuestionSet (string id, bool activeOnly) {
            Guid objGuid = Guid.Empty;
            if (Guid.TryParse (id, out objGuid)) {
                return _mapper.Map<QuestionSetDTO> (await _unitOfWork.QuestionSet.GetQuestionSet (objGuid, activeOnly));
            }
            throw new KeyNotFoundException ("Guid wrong");
        }

        public async Task<IEnumerable<QuestionSetDTO>> GetQuestionSets () {
            var set = await _unitOfWork.QuestionSet.GetAllQuestionSets ();
            var dto = _mapper.Map<IEnumerable<QuestionSetDTO>> (set);
            return dto;
        }

        public async Task<IEnumerable<string>> GetQuestionSetNames () {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            var res = await _unitOfWork.QuestionSet.GetAllQuestionSetsCompany (int.Parse (companyId));
            return res.Select (q => q.Name);
        }

        public async Task<IEnumerable<QuestionSetDTO>> GetQuestionSetOnly () {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN);
            if (isAdmin) {
                return _mapper.Map<IEnumerable<QuestionSetDTO>> (await _unitOfWork.QuestionSet.GetAllQuestionSets ());
            } else {
                return _mapper.Map<IEnumerable<QuestionSetDTO>> (await _unitOfWork.QuestionSet.GetAllQuestionSetsCompany (int.Parse (companyId)));
            }
        }

        public async Task<bool> CreateQuestionSet (QuestionSetDTO Entity) {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            if (Entity.CompanyId == int.Parse (companyId)) {
                var q = _mapper.Map<QuestionSet> (Entity);
                q.active = true;
                _unitOfWork.QuestionSet.CreateQuestionSet (q);
                return await _unitOfWork.SaveAsync ();
            } else {
                return false;
            }
        }

        public async Task<bool> UpdateQuestionSet (QuestionSetDTO Entity) {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            if (Entity.CompanyId == int.Parse (companyId) || int.Parse (companyId) == int.Parse (Environment.GetEnvironmentVariable ("SpinOffCompanyID"))) {
                var oldQuestionSet = await _unitOfWork.QuestionSet.GetQuestionSet (Entity.QuestionSetId, true);
                _mapper.Map (Entity, oldQuestionSet);
                return await _unitOfWork.SaveAsync ();
            } else {
                return false;
            }
        }

        public async Task<bool> DeleteQuestionSet (QuestionSetDTO Entity) {
            var companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            var adminAcceses = (int.Parse (companyId) == 1 && _httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN));
            if (Entity.CompanyId == int.Parse (companyId) || adminAcceses) {
                // _unitOfWork.QuestionSet.Remove (_mapper.Map<QuestionSet> (Entity));
                QuestionSet qset = _mapper.Map<QuestionSet> (Entity);
                qset.active = false;
                _unitOfWork.QuestionSet.UpdateQuestionsSet (qset);

                return await _unitOfWork.SaveAsync ();
            } else {
                return false;
            }
        }
    }
}