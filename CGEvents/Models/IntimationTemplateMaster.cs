using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class IntimationTemplateMaster
    {
        public int Id { get; set; }
        public byte? IntimationTypeId { get; set; }
        public string Filename { get; set; }
        public string MergeFields { get; set; }

        public virtual IntimationTypeMaster IntimationType { get; set; }
    }
}
