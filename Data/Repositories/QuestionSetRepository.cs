﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts.Repositories {
    public class QuestionSetRepository : Repository<QuestionSet>, IQuestionSetRepository {
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

        public void SetQuestionSet (QuestionSet questionSet) {
            throw new System.NotImplementedException ();
        }

        public void UpdateQuestionsSet (QuestionSet questionSet) {
            throw new System.NotImplementedException ();
        }

    }
}