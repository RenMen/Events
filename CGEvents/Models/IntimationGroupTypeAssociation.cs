using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEvents.Models
{
    public partial class IntimationGroupTypeAssociation
    {
       
        public byte IntimationAssocId { get; set; }
        public byte? IntimationGroupId { get; set; }
        public byte? IntimationTypeId { get; set; }

        public virtual IntimationGroupMaster IntimationGroup { get; set; }
        public virtual IntimationTypeMaster IntimationType { get; set; }
    }
}
