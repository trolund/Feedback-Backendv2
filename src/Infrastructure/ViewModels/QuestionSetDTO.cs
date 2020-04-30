using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.ViewModels {
    public class QuestionSetDTO {
        [JsonIgnore]
        public Guid QuestionSetId { get; set; } = Guid.NewGuid ();

        [Required]
        [MaxLength (100)]
        public string Name { get; set; }
        public long Version { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}