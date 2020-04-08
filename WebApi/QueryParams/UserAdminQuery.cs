namespace WebApi.QueryParams {
    public class UserAdminQuery {
        public string searchword { get; set; } = null;
        public int companyConfirmed { get; set; } = -1;
        public int pageNumber { get; set; } = -1;
    }
}