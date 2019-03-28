using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEvents.Models
{
    public partial class IntimationTemplateMaster
    {
        public int Id { get; set; }
        public byte? IntimationTypeId { get; set; }
        public string HtmlContent { get; set; }
        public string MergeFields { get; set; }
        public short? EventId { get; set; }
        public string TemplateName { get; set; }
        public virtual EventMaster Event { get; set; }
        public virtual IntimationTypeMaster IntimationType { get; set; }
    }
}
