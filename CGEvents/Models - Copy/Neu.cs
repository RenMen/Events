using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class Neu
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public byte? AffId { get; set; }
        public string GuestName { get; set; }
        public string EmailId { get; set; }
        public string PemailId { get; set; }
        public bool? IsAttending { get; set; }
        public DateTime? DtSubmit { get; set; }
        public DateTime? DtModified { get; set; }
        public string Oaffliation { get; set; }
        public int Id { get; set; }
        public Guid? UniqId { get; set; }
        public string Company { get; set; }
        public short? EventId { get; set; }
        public bool? AgeLimit { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsGuest { get; set; }
        public string Grelation { get; set; }
        public string Title { get; set; }
        public bool? AgelimitGuest { get; set; }
        public byte? EventGroupId { get; set; }
    }
}
