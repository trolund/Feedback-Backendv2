using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Feedback.Application.Services.Interfaces;
using Feedback.Data;
using Feedback.Models;
using Feedback.viewModels;
using Microsoft.AspNetCore.Http;

namespace Feedback.Application.Services {
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
            return (await _unitOfWork.QuestionSet.GetAll ()).Select (x => x.Name);
        }

        public async Task<IEnumerable<QuestionSetDTO>> GetQuestionSetOnly () {
            return _mapper.Map<IEnumerable<QuestionSetDTO>> (await _unitOfWork.QuestionSet.GetAll ());
        }

        public void CreateQuestionSet (QuestionSetDTO QuestionSet) {
            var q = _mapper.Map<QuestionSet> (QuestionSet);

            _unitOfWork.QuestionSet.Add (q);
            _unitOfWork.Save ();
        }

        public async Task<QuestionSetDTO> UpdateQuestionSet (QuestionSetDTO QuestionSet) {
            var QuestionSetToAdd = _mapper.Map<QuestionSet> (QuestionSet);

            await _unitOfWork.QuestionSet.Add (QuestionSetToAdd);
            await _unitOfWork.SaveAsync ();

            var QuestionSetToReturn = _mapper.Map<QuestionSetDTO> (QuestionSetToAdd);

            return QuestionSetToReturn;
        }

        public void DeleteQuestionSet (QuestionSetDTO QuestionSet) {
            _unitOfWork.QuestionSet.Remove (_mapper.Map<QuestionSet> (QuestionSet));
        }
    }
}