using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repositories.Interface {
    public interface IQuestionSetRepository : IRepository<QuestionSet, Guid> {
        Task<QuestionSet> GetQuestionSet (Guid id);

        Task<IEnumerable<QuestionSet>> GetAllQuestionSets ();

        void CreateQuestionSet (QuestionSet questionSet);

        void UpdateQuestionsSet (QuestionSet questionSet);
    }
}