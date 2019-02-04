using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class SubscriptionMater
    {
        public SubscriptionMater()
        {
            SubscriptionDetails = new HashSet<SubscriptionDetails>();
        }

        public string Fname { get; set; }
        public string Lname { get; set; }
        public string EmailId { get; set; }
        public int Id { get; set; }
        public Guid UniqId { get; set; }
        public bool? Flag { get; set; }
        public int? ReferedBy { get; set; }
        public DateTime? DtStampSubscribe { get; set; }
        public DateTime? DtStampModified { get; set; }
        public DateTime? DtStampUnSubscribe { get; set; }

        public virtual ICollection<SubscriptionDetails> SubscriptionDetails { get; set; }
    }
}
