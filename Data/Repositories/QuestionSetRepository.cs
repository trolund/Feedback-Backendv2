using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Contexts;
using Data.Models;
using Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class QuestionSetRepository : Repository<QuestionSet, Guid>, IQuestionSetRepository {
        public QuestionSetRepository (ApplicationDbContext context) : base (context) { }
        private ApplicationDbContext _context {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<QuestionSet> GetQuestionSet (Guid id) {
            return await _context.QuestionSets.Include (q => q.Questions).SingleOrDefaultAsync (a => a.QuestionSetId == id);
        }

        public async Task<IEnumerable<QuestionSet>> GetAllQuestionSets () {
            // throw if no parametres is provided.
            // filtering
            var collection = _context.QuestionSets as IQueryable<QuestionSet>;
            // pageing
            return await collection.Take (10).Include (set => set.Questions).ToListAsync ();
        }

        public void CreateQuestionSet (QuestionSet questionSet) {
            _context.QuestionSets.Add (questionSet);

        }

        public void UpdateQuestionsSet (QuestionSet questionSet) {
            _context.QuestionSets.Update (questionSet);
        }

    }
}