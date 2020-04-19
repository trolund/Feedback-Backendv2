using System;

namespace Infrastructure.ViewModels {
    public class CompanyConfirmationDTO {
        public Guid UserId { get; set; }
        public int CompanyId { get; set; }
        public bool Status { get; set; }
        public bool delete { get; set; }
    }
}