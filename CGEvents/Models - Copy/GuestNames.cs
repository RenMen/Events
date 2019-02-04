using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class GuestNames
    {
        public string Gname { get; set; }
        public string Grelation { get; set; }
        public int? InvId { get; set; }
        public int Id { get; set; }
        public bool? Flag { get; set; }

        public virtual Ams Inv { get; set; }
    }
}
