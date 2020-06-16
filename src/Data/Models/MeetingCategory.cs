using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Data.Models {

    public class MeetingCategory {
        [JsonIgnore]
        public int MeetingId { get; set; }

        public virtual Meeting meeting { get; set; }
        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}