using System;
namespace Infrastructure.ViewModels {
    public class CategoryDTO {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
    }
}