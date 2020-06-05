using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Business.Services.Interfaces {
    public interface IQuestionSetService {
        Task<QuestionSetDTO> GetQuestionSet (string id, bool activeOnly);
        Task<IEnumerable<QuestionSetDTO>> GetQuestionSets ();

        Task<bool> CreateQuestionSet (QuestionSetDTO QuestionSet);

        Task<bool> UpdateQuestionSet (QuestionSetDTO QuestionSet);

        Task<bool> DeleteQuestionSet (QuestionSetDTO QuestionSet);

        Task<IEnumerable<string>> GetQuestionSetNames ();

        Task<IEnumerable<QuestionSetDTO>> GetQuestionSetOnly ();

    }
}