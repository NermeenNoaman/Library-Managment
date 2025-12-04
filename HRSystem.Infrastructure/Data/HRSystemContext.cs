using HRSystem.BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRSystem.Infrastructure.Data;

public partial class HRSystemContext : DbContext
{
    public HRSystemContext()
    {
    }

    public HRSystemContext(DbContextOptions<HRSystemContext> options)
        : base(options)
    {
    }
    // =======================================================================
    // DbSet Definitions (JTABLES)
    // =======================================================================

    public virtual DbSet<BOOK> BOOKs { get; set; }

    public virtual DbSet<BORROWING> BORROWINGs { get; set; }

    public virtual DbSet<CATEGORY> CATEGORies { get; set; }

    public virtual DbSet<FINE> FINEs { get; set; }

    public virtual DbSet<LIBRARIAN> LIBRARIANs { get; set; }

    public virtual DbSet<LIBRARY> LIBRARies { get; set; }

    public virtual DbSet<LIBRARY_BRANCH> LIBRARY_BRANCHes { get; set; }

    public virtual DbSet<MEMBER> MEMBERs { get; set; }

    public virtual DbSet<REFRESH_TOKEN> REFRESH_TOKENs { get; set; }

    public virtual DbSet<REPORT> REPORTs { get; set; }

    public virtual DbSet<RESERVATION> RESERVATIONs { get; set; }

    public virtual DbSet<USER> USERs { get; set; }



    // =======================================================================
    // Model Creation (Constraints and Relationships)
    // =======================================================================

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BOOK>(entity =>
        {
            entity.HasKey(e => e.book_id).HasName("PK__BOOK__490D1AE10A23D9FB");

            entity.Property(e => e.available_copies).HasDefaultValue(1);
            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.language).HasDefaultValue("English");
            entity.Property(e => e.status).HasDefaultValue("Available");
            entity.Property(e => e.total_copies).HasDefaultValue(1);
            entity.Property(e => e.updated_at).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.category).WithMany(p => p.BOOKs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BOOK__category_i__60A75C0F");
        });

        modelBuilder.Entity<BORROWING>(entity =>
        {
            entity.Property(e => e.status).IsFixedLength();

            entity.HasOne(d => d.book).WithMany(p => p.BORROWINGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BORROWING_BOOK");

            entity.HasOne(d => d.librarian).WithMany(p => p.BORROWINGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BORROWING_LIBRARIAN");

            entity.HasOne(d => d.member).WithMany(p => p.BORROWINGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BORROWING_MEMBER");
        });

        modelBuilder.Entity<CATEGORY>(entity =>
        {
            entity.HasKey(e => e.category_id).HasName("PK__CATEGORY__D54EE9B4871753BA");

            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<FINE>(entity =>
        {
            entity.Property(e => e.payment_status).IsFixedLength();

            entity.HasOne(d => d.borrowing).WithMany(p => p.FINEs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FINE_BORROWING");

            entity.HasOne(d => d.member).WithMany(p => p.FINEs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FINE_MEMBER");
        });

        modelBuilder.Entity<LIBRARIAN>(entity =>
        {
            entity.HasKey(e => e.librarian_id).HasName("PK__LIBRARIA__41AEF2364FE36D24");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARIAN__creat__6FE99F9F");
            entity.Property(e => e.hire_date)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARIAN__hire___6E01572D");
            entity.Property(e => e.status)
                .HasDefaultValue("Active")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARIAN__statu__6EF57B66");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARIAN__updat__70DDC3D8");

            entity.HasOne(d => d.user).WithOne(p => p.LIBRARIAN).HasConstraintName("FK__LIBRARIAN__user___71D1E811");
        });

        modelBuilder.Entity<LIBRARY>(entity =>
        {
            entity.HasKey(e => e.library_id).HasName("PK__LIBRARY__7A2F73CA8363EACA");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARY__created__5629CD9C");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARY__updated__571DF1D5");
        });

        modelBuilder.Entity<LIBRARY_BRANCH>(entity =>
        {
            entity.HasKey(e => e.branch_id).HasName("PK__LIBRARY___E55E37DE13991101");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARY_B__creat__5AEE82B9");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__LIBRARY_B__updat__5BE2A6F2");

            entity.HasOne(d => d.library).WithMany(p => p.LIBRARY_BRANCHes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LIBRARY_BRANCH_LIBRARIAN");

            entity.HasOne(d => d.libraryNavigation).WithMany(p => p.LIBRARY_BRANCHes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LIBRARY_B__libra__5CD6CB2B");
        });

        modelBuilder.Entity<MEMBER>(entity =>
        {
            entity.HasKey(e => e.member_id).HasName("PK__MEMBER__B29B853460F3E148");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__MEMBER__created___68487DD7");
            entity.Property(e => e.membership_type)
                .HasDefaultValue("Standard")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__MEMBER__membersh__66603565");
            entity.Property(e => e.registration_date)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__MEMBER__registra__656C112C");
            entity.Property(e => e.status)
                .HasDefaultValue("Active")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__MEMBER__status__6754599E");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__MEMBER__updated___693CA210");

            entity.HasOne(d => d.user).WithOne(p => p.MEMBER).HasConstraintName("FK__MEMBER__user_id__6A30C649");
        });

        modelBuilder.Entity<REFRESH_TOKEN>(entity =>
        {
            entity.HasKey(e => e.token_id).HasName("PK__REFRESH___CB3C9E179B0515F9");

            entity.Property(e => e.created)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__REFRESH_T__creat__4F7CD00D");
            entity.Property(e => e.revoked).HasAnnotation("Relational:DefaultConstraintName", "DF__REFRESH_T__revok__5070F446");

            entity.HasOne(d => d.user).WithMany(p => p.REFRESH_TOKENs).HasConstraintName("FK__REFRESH_T__user___5165187F");
        });

        modelBuilder.Entity<REPORT>(entity =>
        {
            entity.HasKey(e => e.report_id).HasName("PK__REPORT__779B7C58B2302C7F");

            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.generated_date).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.generated_byNavigation).WithMany(p => p.REPORTs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REPORT__generate__19DFD96B");
        });

        modelBuilder.Entity<RESERVATION>(entity =>
        {
            entity.HasKey(e => e.reservation_id).HasName("PK__RESERVAT__31384C296ED2F1AF");

            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.priority_number).HasDefaultValue(1);
            entity.Property(e => e.reservation_date).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.status).HasDefaultValue("Active");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.book).WithMany(p => p.RESERVATIONs).HasConstraintName("FK__RESERVATI__book___6C190EBB");

            entity.HasOne(d => d.member).WithMany(p => p.RESERVATIONs).HasConstraintName("FK__RESERVATI__membe__0C85DE4D");
        });

        modelBuilder.Entity<USER>(entity =>
        {
            entity.HasKey(e => e.user_id).HasName("PK__USERs__B9BE370FAE5D0CB7");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__USERs__created_a__4AB81AF0");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("(getdate())")
                .HasAnnotation("Relational:DefaultConstraintName", "DF__USERs__updated_a__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

