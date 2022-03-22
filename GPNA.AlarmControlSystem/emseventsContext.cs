using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GPNA.AlarmControlSystem
{
    public partial class emseventsContext : DbContext
    {
        public emseventsContext()
        {
        }

        public emseventsContext(DbContextOptions<emseventsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=GPNA-KF;Database=emsevents;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_Events_EMSEventID")
                    .IsClustered(false);

                entity.Property(e => e.EventId)
                    .ValueGeneratedNever()
                    .HasColumnName("EventID");

                entity.Property(e => e.AccessReason)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Action)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Actor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AlarmHelp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Alarm Help");

                entity.Property(e => e.AreaName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Author)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AuxUnit)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AUX UNIT");

                entity.Property(e => e.Block)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BlockTypeName)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.CardEncodingId).HasColumnName("CardEncodingID");

                entity.Property(e => e.CardHolderFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardHolderId).HasColumnName("CardHolderID");

                entity.Property(e => e.CardHolderLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardHolderType)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Classification)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Criticality)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(132)
                    .IsUnicode(false);

                entity.Property(e => e.Dsaserver)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DSAServer");

                entity.Property(e => e.Eecode).HasColumnName("EECode");

                entity.Property(e => e.EeinitId).HasColumnName("EEInitID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Email Address");

                entity.Property(e => e.EquipmentFullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ExecutionId)
                    .HasColumnType("numeric(20, 0)")
                    .HasColumnName("ExecutionID");

                entity.Property(e => e.GdaconnectionName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GDAConnectionName");

                entity.Property(e => e.Gdaquality).HasColumnName("GDAQuality");

                entity.Property(e => e.GdaserverName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GDAServerName");

                entity.Property(e => e.Length)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Link1)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Link2)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Link3)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.LocationFullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTagName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PointRoleLabel)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PointRoleQualifier)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PrevValue)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryEquipment)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PublicName).HasMaxLength(40);

                entity.Property(e => e.Reason)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Recipe)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RollId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Roll ID");

                entity.Property(e => e.SectionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Section ID");

                entity.Property(e => e.SequenceId).HasColumnName("SequenceID");

                entity.Property(e => e.ShelvedReason)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SignatureMeaning)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SourceEntityName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SourceParameter)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.SourceSort).HasMaxLength(40);

                entity.Property(e => e.Station)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SubConditionName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SuppressionGroup)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.TransitDirection)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.TransitType)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.TsReliable).HasColumnName("TS Reliable");

                entity.Property(e => e.Units)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.ZoneEntered)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
