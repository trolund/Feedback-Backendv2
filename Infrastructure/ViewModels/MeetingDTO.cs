using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.ViewModels {
    public class MeetingDTO {
        public MeetingDTO () { }

        public MeetingDTO (string shortId, string createdBy, string name, DateTime startTime, DateTime endTime, string discription, string topic, string questionsSetId) {
            ShortId = shortId;
            CreatedBy = createdBy;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Discription = discription;
            Topic = topic;
            QuestionsSetId = questionsSetId;
        }

        [JsonIgnore]
        public int MeetingId { get; set; }

        public string ShortId { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }
        // public string Location { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [MaxLength (1500)]
        public string Discription { get; set; }

        [MaxLength (200)]
        public string Topic { get; set; }

        public string QuestionsSetId { get; set; }

        public ICollection<MeetingCategoryDTO> meetingCategories { get; set; }
    }
}