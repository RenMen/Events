using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class Fb
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string PassportName { get; set; }
        public string Paname { get; set; }
        public string Paemail { get; set; }
        public string Patel { get; set; }
        public short? Atime { get; set; }
        public DateTime? Adate { get; set; }
        public short? Dtime { get; set; }
        public DateTime? Ddate { get; set; }
        public int Id { get; set; }
        public Guid? UniqId { get; set; }
        public string AcityName { get; set; }
        public string DcityName { get; set; }
        public bool? IsRequired { get; set; }
        public string EmailId { get; set; }
    }
}
