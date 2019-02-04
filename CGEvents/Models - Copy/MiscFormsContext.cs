using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CGEvents.Models
{
    public partial class MiscFormsContext : DbContext
    {
  
        public MiscFormsContext(DbContextOptions<MiscFormsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AffiliationMaster> AffiliationMaster { get; set; }
        public virtual DbSet<Ams> Ams { get; set; }
        public virtual DbSet<AmstransferDetails> AmstransferDetails { get; set; }
        public virtual DbSet<CategoryMaster> CategoryMaster { get; set; }
        public virtual DbSet<DayParts> DayParts { get; set; }
        public virtual DbSet<EventMaster> EventMaster { get; set; }
        public virtual DbSet<EventProgram> EventProgram { get; set; }
        public virtual DbSet<Fb> Fb { get; set; }
        public virtual DbSet<GuestNames> GuestNames { get; set; }
        public virtual DbSet<Neu> Neu { get; set; }
        public virtual DbSet<QuestionMaster> QuestionMaster { get; set; }
        public virtual DbSet<QuestionnaireDetails> QuestionnaireDetails { get; set; }
        public virtual DbSet<RatingMaster> RatingMaster { get; set; }
        public virtual DbSet<SecretSanta> SecretSanta { get; set; }
        public virtual DbSet<SubscriptionDetails> SubscriptionDetails { get; set; }
        public virtual DbSet<SubscriptionMater> SubscriptionMater { get; set; }
        public virtual DbSet<TransferOptions> TransferOptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["EventsDB"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AffiliationMaster>(entity =>
            {
                entity.HasKey(e => e.AffId);

                entity.Property(e => e.AffId)
                    .HasColumnName("AffID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Affiliation).HasMaxLength(100);
            });

            modelBuilder.Entity<Ams>(entity =>
            {
                entity.ToTable("AMS");

                entity.HasIndex(e => new { e.EmailId, e.EventId })
                    .HasName("UniqInvitee")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcityName)
                    .HasColumnName("ACityName")
                    .HasMaxLength(100);

                entity.Property(e => e.Adate)
                    .HasColumnName("ADate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AgendaFileName).HasMaxLength(150);

                entity.Property(e => e.AirTktFileName).HasMaxLength(250);

                entity.Property(e => e.AlleryDesc).HasMaxLength(250);

                entity.Property(e => e.Atime).HasColumnName("ATime");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.DcityName)
                    .HasColumnName("DCityName")
                    .HasMaxLength(100);

                entity.Property(e => e.Ddate)
                    .HasColumnName("DDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dtModified")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtSubmit)
                    .HasColumnName("dtSubmit")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dtime).HasColumnName("DTime");

                entity.Property(e => e.EmailId)
                    .HasColumnName("emailID")
                    .HasMaxLength(100);

                entity.Property(e => e.EventGroupId).HasColumnName("EventGroupID");

                entity.Property(e => e.EventId).HasColumnName("eventID");

                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(100);

                entity.Property(e => e.HotelChkIn).HasColumnType("date");

                entity.Property(e => e.HotelChkOut).HasColumnType("date");

                entity.Property(e => e.IcsFileName)
                    .HasColumnName("icsFileName")
                    .HasMaxLength(150);

                entity.Property(e => e.Inclass)
                    .HasColumnName("INClass")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Indate)
                    .HasColumnName("INDate")
                    .HasColumnType("date");

                entity.Property(e => e.IndvDeadline).HasColumnType("datetime");

                entity.Property(e => e.Ineta).HasColumnName("INETA");

                entity.Property(e => e.Inetd).HasColumnName("INETD");

                entity.Property(e => e.InflightNo)
                    .HasColumnName("INFlightNo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Insec)
                    .HasColumnName("INSec")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.IsNew)
                    .HasColumnName("isNew")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Lname)
                    .HasColumnName("LName")
                    .HasMaxLength(100);

                entity.Property(e => e.NoOfCoAttendee).HasColumnName("noOfCoAttendee");

                entity.Property(e => e.Obclass)
                    .HasColumnName("OBClass")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Obdate)
                    .HasColumnName("OBDate")
                    .HasColumnType("date");

                entity.Property(e => e.Obeta).HasColumnName("OBETA");

                entity.Property(e => e.Obetd).HasColumnName("OBETD");

                entity.Property(e => e.ObflightNo)
                    .HasColumnName("OBFlightNo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Obsec)
                    .HasColumnName("OBSec")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.OwnArrDate).HasColumnType("date");

                entity.Property(e => e.OwnArrFlightNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnArrTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnDepDate).HasColumnType("date");

                entity.Property(e => e.OwnDepFlightNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnDepTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Paemail)
                    .HasColumnName("PAEmail")
                    .HasMaxLength(100);

                entity.Property(e => e.Paname)
                    .HasColumnName("PAName")
                    .HasMaxLength(100);

                entity.Property(e => e.PassportName).HasMaxLength(200);

                entity.Property(e => e.Patel)
                    .HasColumnName("PATel")
                    .HasMaxLength(100);

                entity.Property(e => e.Position).HasMaxLength(250);

                entity.Property(e => e.Tempemail)
                    .HasColumnName("tempemail")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'rkumar.mr@choueirigroup.com')");

                entity.Property(e => e.UniqId)
                    .HasColumnName("UniqID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.VisaFileName).HasMaxLength(250);
                entity.HasOne(d => d.EventIdNavigation)
                   .WithMany(p => p.Ams)
                   .HasForeignKey(d => d.EventId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_AMS_EventMaster");
            });

            modelBuilder.Entity<AmstransferDetails>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.ToTable("AMSTransferDetails");

                entity.Property(e => e.Tid).HasColumnName("TID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TransferId).HasColumnName("TransferID");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.AmstransferDetails)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AMSTransferDetails_AMS");
            });

            modelBuilder.Entity<CategoryMaster>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<DayParts>(entity =>
            {
                entity.HasKey(e => e.TimeId);

                entity.Property(e => e.TimeId).HasColumnName("TimeID");

                entity.Property(e => e.Time)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventMaster>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventId).HasColumnName("EventID").ValueGeneratedOnAdd();

                entity.Property(e => e.AckText)
                    .HasColumnName("ackText")
                    .HasMaxLength(100);

                entity.Property(e => e.AirTktLoc).HasMaxLength(50);

                entity.Property(e => e.DispName)
                    .HasColumnName("dispName")
                    .HasMaxLength(150);
                   
                
                entity.Property(e => e.EventAgendaUrl)
                    .HasColumnName("EventAgendaURL")
                    .HasMaxLength(1000);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventDateTo).HasColumnType("datetime");

                entity.Property(e => e.EventDispName).HasMaxLength(100);

                entity.Property(e => e.EventName).HasMaxLength(100);

                entity.Property(e => e.EventUrl)
                    .HasColumnName("eventURL")
                    .HasMaxLength(1000);

                entity.Property(e => e.FormDeadline).HasColumnType("datetime");

                entity.Property(e => e.Hotel).HasMaxLength(50);

                entity.Property(e => e.IcsFileName)
                    .HasColumnName("icsFileName")
                    .HasMaxLength(150);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MailMergeLink).HasMaxLength(1000);

                entity.Property(e => e.ReachVenueByLatest).HasMaxLength(10);

                entity.Property(e => e.RepLink).HasMaxLength(1000);

                entity.Property(e => e.Subject).HasMaxLength(500);

                entity.Property(e => e.TrsfrOpt).HasColumnName("trsfrOpt");

                entity.Property(e => e.Venue).HasMaxLength(250);

                entity.Property(e => e.VisaLoc).HasMaxLength(50);
            });

            modelBuilder.Entity<EventProgram>(entity =>
            {
                entity.ToTable("eventProgram");

                entity.Property(e => e.EventProgramId).HasColumnName("eventProgramID");

                entity.Property(e => e.EventProgram1).HasColumnName("eventProgram");

                entity.Property(e => e.EventProgramGroupId).HasColumnName("EventProgramGroupID");

                entity.Property(e => e.Eventid).HasColumnName("eventid");
            });

            modelBuilder.Entity<Fb>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcityName)
                    .HasColumnName("ACityName")
                    .HasMaxLength(100);

                entity.Property(e => e.Adate)
                    .HasColumnName("ADate")
                    .HasColumnType("date");

                entity.Property(e => e.Atime).HasColumnName("ATime");

                entity.Property(e => e.DcityName)
                    .HasColumnName("DCityName")
                    .HasMaxLength(100);

                entity.Property(e => e.Ddate)
                    .HasColumnName("DDate")
                    .HasColumnType("date");

                entity.Property(e => e.Dtime).HasColumnName("DTime");

                entity.Property(e => e.EmailId)
                    .HasColumnName("emailID")
                    .HasMaxLength(100);

                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(100);

                entity.Property(e => e.Lname)
                    .HasColumnName("LName")
                    .HasMaxLength(100);

                entity.Property(e => e.Paemail)
                    .HasColumnName("PAEmail")
                    .HasMaxLength(100);

                entity.Property(e => e.Paname)
                    .HasColumnName("PAName")
                    .HasMaxLength(100);

                entity.Property(e => e.PassportName).HasMaxLength(200);

                entity.Property(e => e.Patel)
                    .HasColumnName("PATel")
                    .HasMaxLength(100);

                entity.Property(e => e.UniqId)
                    .HasColumnName("UniqID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<GuestNames>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.Gname)
                    .HasColumnName("GName")
                    .HasMaxLength(50);

                entity.Property(e => e.Grelation)
                    .HasColumnName("GRelation")
                    .HasMaxLength(100);

                entity.Property(e => e.InvId).HasColumnName("invID");

                entity.HasOne(d => d.Inv)
                    .WithMany(p => p.GuestNames)
                    .HasForeignKey(d => d.InvId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GuestNames_AMS");
            });

            modelBuilder.Entity<Neu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AffId).HasColumnName("AffID");

                entity.Property(e => e.AgeLimit).HasColumnName("ageLimit");

                entity.Property(e => e.AgelimitGuest).HasColumnName("agelimitGuest");

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.DtModified)
                    .HasColumnName("dtModified")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtSubmit)
                    .HasColumnName("dtSubmit")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .HasMaxLength(75);

                entity.Property(e => e.EventGroupId).HasColumnName("eventGroupId");

                entity.Property(e => e.EventId).HasColumnName("eventID");

                entity.Property(e => e.Fname).HasMaxLength(100);

                entity.Property(e => e.Grelation)
                    .HasColumnName("GRelation")
                    .HasMaxLength(50);

                entity.Property(e => e.GuestName).HasMaxLength(100);

                entity.Property(e => e.IsGuest).HasColumnName("isGuest");

                entity.Property(e => e.IsNew).HasColumnName("isNew");

                entity.Property(e => e.Lname).HasMaxLength(100);

                entity.Property(e => e.Oaffliation)
                    .HasColumnName("OAffliation")
                    .HasMaxLength(100);

                entity.Property(e => e.PemailId)
                    .HasColumnName("PEmailID")
                    .HasMaxLength(75);

                entity.Property(e => e.Title).HasMaxLength(25);

                entity.Property(e => e.UniqId)
                    .HasColumnName("UniqID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<QuestionMaster>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.Property(e => e.QuestionId)
                    .HasColumnName("QuestionID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<QuestionnaireDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.RespondentId).HasColumnName("RespondentID");
            });

            modelBuilder.Entity<RatingMaster>(entity =>
            {
                entity.HasKey(e => e.RatingId);

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.Rating).HasMaxLength(250);
            });

            modelBuilder.Entity<SecretSanta>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Company).HasMaxLength(255);

                entity.Property(e => e.Dept).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.Floor).HasMaxLength(50);

                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(255);

                entity.Property(e => e.Lname)
                    .HasColumnName("LName")
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SecretSanta1)
                    .HasColumnName("SecretSanta")
                    .HasMaxLength(255);

                entity.Property(e => e.UniqId)
                    .HasColumnName("uniqID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<SubscriptionDetails>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK_SubscriptionDetails_1");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.SubscriptionDetails)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_SubscriptionDetails_SubscriptionMater");
            });

            modelBuilder.Entity<SubscriptionMater>(entity =>
            {
                entity.HasIndex(e => e.EmailId)
                    .HasName("IX_SubscriptionMater")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DtStampModified)
                    .HasColumnName("dtStampModified")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtStampSubscribe)
                    .HasColumnName("dtStampSubscribe")
                    .HasColumnType("datetime");

                entity.Property(e => e.DtStampUnSubscribe)
                    .HasColumnName("dtStampUnSubscribe")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .HasMaxLength(100);

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Fname)
                    .HasColumnName("fname")
                    .HasMaxLength(150);

                entity.Property(e => e.Lname).HasMaxLength(150);

                entity.Property(e => e.ReferedBy).HasColumnName("referedBy");

                entity.Property(e => e.UniqId)
                    .HasColumnName("uniqID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TransferOptions>(entity =>
            {
                entity.HasKey(e => e.TransferId);

                entity.Property(e => e.TransferId).HasColumnName("TransferID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.GroupId).HasColumnName("groupID");

                entity.Property(e => e.MailMergeText).HasMaxLength(160);

                entity.Property(e => e.TransferText).HasMaxLength(100);
            });
        }
    }
}
