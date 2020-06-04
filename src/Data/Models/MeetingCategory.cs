using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models {
    public class MeetingCategory {
        // public Guid MeetingCategoryId { get; set; }
        public int MeetingId { get; set; }

        [JsonIgnore]
        public Meeting meeting { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}