using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Service.Models;

public partial class DbContextHelper 
{


    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<Numberingschema> Numberingschemas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SystemMetadatum> SystemMetadata { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__A1682FC575DC7A96");

            entity.ToTable("Branch");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BranchName).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971CAC3286C63E");

            entity.ToTable("Company");

            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CinNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Facebook)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FooterNote)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.GstNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IfscCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Instagram)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.InvoicePrefix)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("INV");
            entity.Property(e => e.InvoiceStartNumber).HasDefaultValue(1001);
            entity.Property(e => e.LogoPath).IsUnicode(false);
            entity.Property(e => e.LogonName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PanNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SignaturePath).IsUnicode(false);
            entity.Property(e => e.SignaturenName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tagline)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TermsConditions).IsUnicode(false);
            entity.Property(e => e.UpiId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Website)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Whatsapp)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Youtube)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyAddress>(entity =>
        {
            entity.HasKey(e => e.CompanyAddressId).HasName("PK__CompanyA__3EEBC1D61FA38FDE");

            entity.ToTable("CompanyAddress");

            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Area)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Label)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Landmark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyAddress_Company");
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.HasKey(e => e.Functionid).HasName("PK__FUNCTION__CEA3E564844D1972");

            entity.ToTable("FUNCTION");

            entity.Property(e => e.Functionid).HasColumnName("functionid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdon)
                .HasColumnType("datetime")
                .HasColumnName("createdon");
            entity.Property(e => e.Functionname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("functionname");
            entity.Property(e => e.Functionurl)
                .HasMaxLength(300)
                .HasColumnName("functionurl");
            entity.Property(e => e.Isapprovalneeded).HasColumnName("isapprovalneeded");
            entity.Property(e => e.Isexternal).HasColumnName("isexternal");
            entity.Property(e => e.Menuicon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("menuicon");
            entity.Property(e => e.Mobilescreen).HasColumnName("mobilescreen");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiedon)
                .HasColumnType("datetime")
                .HasColumnName("modifiedon");
            entity.Property(e => e.Parentid).HasColumnName("parentid");
            entity.Property(e => e.Rellink)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("rellink");
            entity.Property(e => e.Screenorder).HasColumnName("screenorder");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Numberingschema>(entity =>
        {
            entity.HasKey(e => e.Numberschemaid).HasName("pk_Numberingschema_numberschemaid");

            entity.ToTable("numberingschema");

            entity.Property(e => e.Numberschemaid).HasColumnName("numberschemaid");
            entity.Property(e => e.Createdon).HasColumnType("datetime");
            entity.Property(e => e.Documentid).HasColumnName("documentid");
            entity.Property(e => e.Isdateformat)
                .HasDefaultValue(false)
                .HasColumnName("isdateformat");
            entity.Property(e => e.Modifiedon).HasColumnType("datetime");
            entity.Property(e => e.Prefix)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prefix");
            entity.Property(e => e.Rv)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("rv");
            entity.Property(e => e.Sequencename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sequencename");
            entity.Property(e => e.Suffix)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("suffix");
            entity.Property(e => e.Symbolid).HasColumnName("symbolid");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A567F55E9");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SystemMetadatum>(entity =>
        {
            entity.HasKey(e => e.MetadataId).HasName("PK__SystemMe__66106FD99F40DD17");

            entity.HasIndex(e => new { e.Category, e.IsActive }, "IX_SystemMetadata_Category");

            entity.HasIndex(e => new { e.Category, e.Code }, "IX_SystemMetadata_Category_Code");

            entity.HasIndex(e => new { e.Category, e.Code }, "UQ_Metadata_Category_Code").IsUnique();

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsSystem).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C6FFEDD64");

            entity.Property(e => e.AvatarName).HasMaxLength(250);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBY");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });
        modelBuilder.HasSequence("BranchSeq");
        modelBuilder.HasSequence("RoleSeq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
