using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace Data.Models {
    public class QuestionSet : BaseEntity {
        public Guid QuestionSetId { get; set; }

        [Required]
        public bool active { get; set; }

        [Required]
        [MaxLength (100)]
        public string Name { get; set; }
        public long Version { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [JsonIgnore]
        public Company Company { get; set; }

        public List<Question> Questions { get; set; }

    }
}