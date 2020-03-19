using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Feedback.Domain.Models;

namespace Feedback.Models {
    public class Meeting : BaseEntity {
        [Key]
        public int MeetingId { get; set; }

        //public string Location { get; set; }

        [Required]
        [MaxLength (150)]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public ICollection<MeetingCategory> meetingCategories { get; set; }

        // [Required]
        // public DateTime RealEndTime { get; set; }
        // public DateTime RealStartTime { get; set; }

        [MaxLength (1500)]
        public string Discription { get; set; }

        [MaxLength (100)]
        public string Topic { get; set; }

        public Guid QuestionsSetId { get; set; }
        public QuestionSet QuestionsSet { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}