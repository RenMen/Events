using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CGEvents.Models
{
    public partial class AffiliationMaster
    {
        
        public byte AffId { get; set; }
        public string Affiliation { get; set; }
    }
}
