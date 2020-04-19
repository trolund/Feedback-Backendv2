using System;

namespace Infrastructure.ViewModels {
    public class UserAdminDTO {
        public Guid Id { get; set; }
        public int CompanyId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool delete { get; set; }
        public bool CompanyConfirmed { get; set; }
    }
}