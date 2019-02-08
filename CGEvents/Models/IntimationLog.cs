using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEvents.Models
{
    public partial class IntimationLog
    {
       
        public int IntimationId { get; set; }
        public int? InviteeId { get; set; }
        public byte? IntimationTypeId { get; set; }
        public DateTime? DtSend { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }

        public virtual IntimationTypeMaster IntimationType { get; set; }
        public virtual Ams Invitee { get; set; }
    }
}
