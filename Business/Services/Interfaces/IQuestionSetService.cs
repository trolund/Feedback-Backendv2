using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface IQuestionSetService {
        Task<QuestionSetDTO> GetQuestionSet (string id);
        Task<IEnumerable<QuestionSetDTO>> GetQuestionSets ();

        void CreateQuestionSet (QuestionSetDTO QuestionSet);

        Task<QuestionSetDTO> UpdateQuestionSet (QuestionSetDTO QuestionSet);

        void DeleteQuestionSet (QuestionSetDTO QuestionSet);

        Task<IEnumerable<string>> GetQuestionSetNames ();

        Task<IEnumerable<QuestionSetDTO>> GetQuestionSetOnly ();

    }
}