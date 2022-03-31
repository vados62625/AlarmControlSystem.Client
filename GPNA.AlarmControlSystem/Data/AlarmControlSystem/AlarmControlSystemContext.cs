using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GPNA.AlarmControlSystem.Data.AlarmControlSystem
{
    public partial class AlarmControlSystemContext : DbContext
    {
        public AlarmControlSystemContext()
        {
        }

        public AlarmControlSystemContext(DbContextOptions<AlarmControlSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=GPNA-KF;Database=AlarmControlSystem;Trusted_Connection=True;");
                optionsBuilder.UseNpgsql("Host=192.168.108.199;Database=AlarmControlSystem;Username=postgres;Password=!Q2wAZsx");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Event");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.Description).HasMaxLength(130);

                entity.Property(e => e.LastTime).HasColumnType("time(6) without time zone");

                entity.Property(e => e.TagName).HasMaxLength(40);

                entity.Property(e => e.Value).HasMaxLength(24);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Tag");

                entity.Property(e => e.ActionFieldArm)
                    .HasMaxLength(500)
                    .HasColumnName("ActionFieldARM");

                entity.Property(e => e.ActionFieldOperator).HasMaxLength(500);

                entity.Property(e => e.Consequence).HasMaxLength(500);

                entity.Property(e => e.Description).HasMaxLength(140);

                entity.Property(e => e.Inform).HasMaxLength(200);

                entity.Property(e => e.Position).HasMaxLength(20);

                entity.Property(e => e.Priority).HasMaxLength(20);

                entity.Property(e => e.ReactionTime).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.TagName).HasMaxLength(40);

                entity.Property(e => e.Unit).HasMaxLength(40);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
