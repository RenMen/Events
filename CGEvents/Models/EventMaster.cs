using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEvents.Models
{
    public partial class EventMaster
    {
        public EventMaster()
        {
            Ams = new HashSet<Ams>();
        }


        [Key]
        public short EventId { get; set; }
        [Required(ErrorMessage = "Event Name Required")]
        public string EventName { get; set; }
        public string EventDispName { get; set; }
        public string EventUrl { get; set; }
        [Required(ErrorMessage = "Event Start Date Required")]
        public DateTime? EventDate { get; set; }
        [Required(ErrorMessage = "Event Deadline Required")]
        public DateTime? FormDeadline { get; set; }
        public bool? IsActive { get; set; }
        public string RepLink { get; set; }
        public string AirTktLoc { get; set; }
        public string AckText { get; set; }
        public string DispName { get; set; }
        public byte? TrsfrOpt { get; set; }
        [Required(ErrorMessage = "Venue Required")]
        public string Venue { get; set; }
        public string ReachVenueByLatest { get; set; }
        public string Hotel { get; set; }
        public string VisaLoc { get; set; }
        public string IcsFileName { get; set; }
        public string MailMergeLink { get; set; }
        [Required(ErrorMessage = "Event End Date Required")]
        public DateTime? EventDateTo { get; set; }
        public string EventAgendaUrl { get; set; }
        [Required(ErrorMessage = "Subject Required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Creative Required")]
        public string MailHeader { get; set; }
        [Required(ErrorMessage = "Body Text Required")]
        public string MailBody { get; set; }
        [Required(ErrorMessage = "Signature Required")]
        public string MailSignature { get; set; }
        public virtual ICollection<Ams> Ams { get; set; }
    }
}
