using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class AmstransferDetails
    {
        public int? Id { get; set; }
        public int? TransferId { get; set; }
        public int Tid { get; set; }

        public virtual Ams IdNavigation { get; set; }
    }
}
