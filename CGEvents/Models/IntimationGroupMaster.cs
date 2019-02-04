using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class IntimationGroupMaster
    {
        public IntimationGroupMaster()
        {
            IntimationGroupTypeAssociation = new HashSet<IntimationGroupTypeAssociation>();
        }

        public byte IntimationGroupId { get; set; }
        public string IntimationGroupName { get; set; }

        public virtual ICollection<IntimationGroupTypeAssociation> IntimationGroupTypeAssociation { get; set; }
    }
}
