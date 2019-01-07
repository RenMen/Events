using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class TransferOptions
    {
        public int TransferId { get; set; }
        public string TransferText { get; set; }
        public short? EventId { get; set; }
        public string MailMergeText { get; set; }
        public byte? GroupId { get; set; }
    }
}
