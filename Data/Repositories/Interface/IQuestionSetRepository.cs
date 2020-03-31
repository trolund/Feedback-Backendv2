using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repositories.Interface {
    public interface IQuestionSetRepository : IRepository<QuestionSet> {
        Task<QuestionSet> GetQuestionSet (Guid id);

        Task<IEnumerable<QuestionSet>> GetAllQuestionSets ();

        void SetQuestionSet (QuestionSet questionSet);

        void UpdateQuestionsSet (QuestionSet questionSet);
    }
}