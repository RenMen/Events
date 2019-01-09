using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEvents.Models
{
    public partial class Ams
    {
        public Ams()
        {
            AmstransferDetails = new HashSet<AmstransferDetails>();
            GuestNames = new HashSet<GuestNames>();
            
        }
        [Required(ErrorMessage = "First Name Required")]
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string PassportName { get; set; }
        public string Paname { get; set; }
        public string Paemail { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
        public short? EventId { get; set; }
        public short? EventGroupId { get; set; }
        public string Company { get; set; }
        public Guid? UniqId { get; set; }
        public string Patel { get; set; }
        public short? Atime { get; set; }
        public DateTime? Adate { get; set; }
        public short? Dtime { get; set; }
        public DateTime? Ddate { get; set; }
        public int Id { get; set; }
        public string AcityName { get; set; }
        public string DcityName { get; set; }
        public bool? IsRequired { get; set; }
        public DateTime? OwnArrDate { get; set; }
        public string OwnArrTime { get; set; }
        public string OwnArrFlightNo { get; set; }
        public DateTime? OwnDepDate { get; set; }
        public string OwnDepTime { get; set; }
        public string OwnDepFlightNo { get; set; }
        public DateTime? HotelChkIn { get; set; }
        public DateTime? HotelChkOut { get; set; }
        public bool? IsVisaReq { get; set; }
        public DateTime? DtSubmit { get; set; }
        public bool? IsAttending { get; set; }
        public byte? AttendingOpt { get; set; }
        public bool? IsFollowupReq { get; set; }
        public bool? IsHotelReq { get; set; }
        public string Comments { get; set; }
        public bool? IsTransferReq { get; set; }
        public string Tempemail { get; set; }
        public string ObflightNo { get; set; }
        public DateTime? Obdate { get; set; }
        public TimeSpan? Obetd { get; set; }
        public TimeSpan? Obeta { get; set; }
        public string Obclass { get; set; }
        public string InflightNo { get; set; }
        public DateTime? Indate { get; set; }
        public TimeSpan? Inetd { get; set; }
        public TimeSpan? Ineta { get; set; }
        public string Inclass { get; set; }
        public string Obsec { get; set; }
        public string Insec { get; set; }
        public string AirTktFileName { get; set; }
        public bool? IsNew { get; set; }
        public byte? IsVisaReqOpt { get; set; }
        public string VisaFileName { get; set; }
        public string City { get; set; }
        public byte? Starter { get; set; }
        public byte? Grill { get; set; }
        public byte? Dessert { get; set; }
        public DateTime? DtModified { get; set; }
        public byte? NoOfCoAttendee { get; set; }
        public bool? FdAllergy { get; set; }
        public string AlleryDesc { get; set; }
        public string AgendaFileName { get; set; }
        public string IcsFileName { get; set; }
        public DateTime? IndvDeadline { get; set; }
        public string Position { get; set; }
        public bool? ActualAttendance { get; set; }
        public virtual EventMaster EventIdNavigation { get; set; }
        public virtual ICollection<AmstransferDetails> AmstransferDetails { get; set; }
        public virtual ICollection<GuestNames> GuestNames { get; set; }
     
        
    }
}
