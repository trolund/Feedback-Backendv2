using System;

namespace Data.Models {
    public class Subscription : BaseEntity {
        public Guid SubscriptionId { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime PayDay { get; set; }
    }
}