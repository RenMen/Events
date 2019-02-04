using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class IntimationTypeMaster
    {
        public IntimationTypeMaster()
        {
            IntimationGroupTypeAssociation = new HashSet<IntimationGroupTypeAssociation>();
            IntimationLog = new HashSet<IntimationLog>();
            IntimationTemplateMaster = new HashSet<IntimationTemplateMaster>();
        }

        public byte IntimationTypeId { get; set; }
        public string IntimationType { get; set; }

        public virtual ICollection<IntimationGroupTypeAssociation> IntimationGroupTypeAssociation { get; set; }
        public virtual ICollection<IntimationLog> IntimationLog { get; set; }
        public virtual ICollection<IntimationTemplateMaster> IntimationTemplateMaster { get; set; }
    }
}
