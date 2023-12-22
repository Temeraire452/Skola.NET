using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Skola.NET.Models;

public partial class SkolaContext : DbContext
{
    public SkolaContext()
    {
    }

    public SkolaContext(DbContextOptions<SkolaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Betyg> Betygs { get; set; }

    public virtual DbSet<Klass> Klasses { get; set; }

    public virtual DbSet<Kur> Kurs { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Skola;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Betyg>(entity =>
        {
            entity.HasKey(e => e.BetygId).HasName("PK__Betyg__E90ED0489B540EDF");

            entity.ToTable("Betyg");

            entity.Property(e => e.BetygId)
                .ValueGeneratedNever()
                .HasColumnName("BetygID");
            entity.Property(e => e.Betyg1)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Betyg");
            entity.Property(e => e.Datum).HasColumnType("date");
            entity.Property(e => e.Fkkurs).HasColumnName("FKKurs");
            entity.Property(e => e.FkpersonalId).HasColumnName("FKPersonalID");
            entity.Property(e => e.FkstudentId).HasColumnName("FKStudentID");

            entity.HasOne(d => d.FkkursNavigation).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.Fkkurs)
                .HasConstraintName("FK__Betyg__FKKurs__412EB0B6");

            entity.HasOne(d => d.Fkpersonal).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.FkpersonalId)
                .HasConstraintName("FK__Betyg__FKPersona__4222D4EF");

            entity.HasOne(d => d.Fkstudent).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.FkstudentId)
                .HasConstraintName("FK__Betyg__FKStudent__403A8C7D");
        });

        modelBuilder.Entity<Klass>(entity =>
        {
            entity.HasKey(e => e.KlassId).HasName("PK__Klass__CF47A60D44DC1547");

            entity.ToTable("Klass");

            entity.Property(e => e.KlassId)
                .ValueGeneratedNever()
                .HasColumnName("KlassID");
            entity.Property(e => e.Klassnamn)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Kur>(entity =>
        {
            entity.HasKey(e => e.KursId).HasName("PK__Kurs__BCCFFF3BD46C01F4");

            entity.Property(e => e.KursId)
                .ValueGeneratedNever()
                .HasColumnName("KursID");
            entity.Property(e => e.Kursnamn)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.PersonalId).HasName("PK__Personal__283437136701135F");

            entity.ToTable("Personal");

            entity.Property(e => e.PersonalId)
                .ValueGeneratedNever()
                .HasColumnName("PersonalID");
            entity.Property(e => e.Befattning)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Namn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Personnummer)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79B45DEFC7");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("StudentID");
            entity.Property(e => e.FkklassId).HasColumnName("FKKlassID");
            entity.Property(e => e.Namn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Personnummer)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Fkklass).WithMany(p => p.Students)
                .HasForeignKey(d => d.FkklassId)
                .HasConstraintName("FK__Student__FKKlass__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
