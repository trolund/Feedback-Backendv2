using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Business.Services {
    public class QuestionSetService : IQuestionSetService {
        private UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionSetService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWork (context, httpContextAccessor, mapper);
            _mapper = mapper;
        }

        public async Task<QuestionSetDTO> GetQuestionSet (string id) {
            Guid objGuid = Guid.Empty;
            if (Guid.TryParse (id, out objGuid)) {
                return _mapper.Map<QuestionSetDTO> (await _unitOfWork.QuestionSet.GetQuestionSet (objGuid));
            }
            throw new KeyNotFoundException ("Guid wrong");
        }

        public async Task<IEnumerable<QuestionSetDTO>> GetQuestionSets () {
            var set = await _unitOfWork.QuestionSet.GetAllQuestionSets ();
            var dto = _mapper.Map<IEnumerable<QuestionSetDTO>> (set);
            return dto;
        }

        public async Task<IEnumerable<string>> GetQuestionSetNames () {
            var res = await _unitOfWork.QuestionSet.GetAll ();
            return res.Select (q => q.Name);
        }

        public async Task<IEnumerable<QuestionSetDTO>> GetQuestionSetOnly () {
            return _mapper.Map<IEnumerable<QuestionSetDTO>> (await _unitOfWork.QuestionSet.GetAll ());
        }

        public void CreateQuestionSet (QuestionSetDTO Entity) {
            var q = _mapper.Map<QuestionSet> (Entity);

            _unitOfWork.QuestionSet.Add (q);
            _unitOfWork.Save ();
        }

        public async Task<QuestionSetDTO> UpdateQuestionSet (QuestionSetDTO Entity) {
            var QuestionSetToAdd = _mapper.Map<QuestionSet> (Entity);

            await _unitOfWork.QuestionSet.Add (QuestionSetToAdd);
            await _unitOfWork.SaveAsync ();

            var QuestionSetToReturn = _mapper.Map<QuestionSetDTO> (QuestionSetToAdd);

            return QuestionSetToReturn;
        }

        public void DeleteQuestionSet (QuestionSetDTO Entity) {
            _unitOfWork.QuestionSet.Remove (_mapper.Map<QuestionSet> (Entity));
        }
    }
}