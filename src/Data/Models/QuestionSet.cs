using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models {
    public class QuestionSet : BaseEntity {
        public Guid QuestionSetId { get; set; }

        [Required]
        [MaxLength (100)]
        public string Name { get; set; }
        public long Version { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public List<Question> Questions { get; set; }

    }
}