using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Feedback.Domain.Models
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column("CreatedBy")]
        [Display(Name = "Creator")]
        public string CreatedBy { get; set; }

        [Column("ModifiedBy")]
        [Display(Name = "Modifier")]
        public string ModifiedBy { get; set; }
    }
}