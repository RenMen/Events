using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class SubscriptionDetails
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int Uid { get; set; }

        public virtual SubscriptionMater IdNavigation { get; set; }
    }
}
