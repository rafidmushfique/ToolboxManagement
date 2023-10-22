using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LILI_TTS.Temp_Models
{
    public partial class Temp_CommonDbContext : DbContext
    {
        public virtual DbSet<TblAction> TblAction { get; set; }
        public virtual DbSet<TblActionType> TblActionType { get; set; }
        public virtual DbSet<TblArea> TblArea { get; set; }
        public virtual DbSet<TblAttachmentType> TblAttachmentType { get; set; }
        public virtual DbSet<TblBusinessSetupInfo> TblBusinessSetupInfo { get; set; }
        public virtual DbSet<TblMenuList> TblMenuList { get; set; }
        public virtual DbSet<TblRegion> TblRegion { get; set; }
        public virtual DbSet<TblToolAssign> TblToolAssign { get; set; }
        public virtual DbSet<TblToolsSetup> TblToolsSetup { get; set; }
        public virtual DbSet<TblTsasetup> TblTsasetup { get; set; }
        public virtual DbSet<TblTsatoolReceiveAttachment> TblTsatoolReceiveAttachment { get; set; }

        // Unable to generate entity type for table 'dbo.AspNetRoleClaims'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetRoles'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUserClaims'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUserLogins'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUserRoles'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUsers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUserTokens'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MenuMaster'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tblMenu'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server= 192.168.100.60;Database=dbToolsManagement;user id=sa;password=dataport;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAction>(entity =>
            {
                entity.ToTable("tblAction");

                entity.HasIndex(e => e.ActionCode)
                    .HasName("IX_tblAction")
                    .IsUnique();

                entity.Property(e => e.ActionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblActionType>(entity =>
            {
                entity.ToTable("tblActionType");

                entity.HasIndex(e => e.ActionTypeCode)
                    .HasName("IX_tblActionType")
                    .IsUnique();

                entity.Property(e => e.ActionTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblArea>(entity =>
            {
                entity.ToTable("tblArea");

                entity.HasIndex(e => e.AreaCode)
                    .HasName("IX_tblArea")
                    .IsUnique();

                entity.Property(e => e.AreaCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblAttachmentType>(entity =>
            {
                entity.ToTable("tblAttachmentType");

                entity.HasIndex(e => e.AttachmentTypeCode)
                    .HasName("IX_tblAttachmentType")
                    .IsUnique();

                entity.Property(e => e.AttachmentTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AttachmentTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBusinessSetupInfo>(entity =>
            {
                entity.ToTable("tblBusinessSetupInfo");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenuList>(entity =>
            {
                entity.ToTable("tblMenuList");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRegion>(entity =>
            {
                entity.ToTable("tblRegion");

                entity.HasIndex(e => e.RegionCode)
                    .HasName("IX_tblRegion")
                    .IsUnique();

                entity.Property(e => e.RegionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblToolAssign>(entity =>
            {
                entity.ToTable("tblToolAssign");

                entity.HasIndex(e => e.AssignCode)
                    .HasName("IX_tblToolAssign");

                entity.Property(e => e.ActionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.ActionTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AssignCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToolCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Tsacode)
                    .IsRequired()
                    .HasColumnName("TSACode")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.ActionCodeNavigation)
                    .WithMany(p => p.TblToolAssign)
                    .HasPrincipalKey(p => p.ActionCode)
                    .HasForeignKey(d => d.ActionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToolAssign_tblAction");

                entity.HasOne(d => d.ActionTypeCodeNavigation)
                    .WithMany(p => p.TblToolAssign)
                    .HasPrincipalKey(p => p.ActionTypeCode)
                    .HasForeignKey(d => d.ActionTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToolAssign_tblActionType");

                entity.HasOne(d => d.ToolCodeNavigation)
                    .WithMany(p => p.TblToolAssign)
                    .HasPrincipalKey(p => p.ToolCode)
                    .HasForeignKey(d => d.ToolCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToolAssign_tblToolsSetup");

                entity.HasOne(d => d.TsacodeNavigation)
                    .WithMany(p => p.TblToolAssign)
                    .HasPrincipalKey(p => p.Tsacode)
                    .HasForeignKey(d => d.Tsacode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToolAssign_tblTSASetup");
            });

            modelBuilder.Entity<TblToolsSetup>(entity =>
            {
                entity.ToTable("tblToolsSetup");

                entity.HasIndex(e => e.ToolCode)
                    .HasName("IX_tblToolsSetup")
                    .IsUnique();

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToolCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ToolName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTsasetup>(entity =>
            {
                entity.ToTable("tblTSASetup");

                entity.HasIndex(e => e.Tsacode)
                    .HasName("IX_tblTSASetup")
                    .IsUnique();

                entity.Property(e => e.AreaCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Designation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegionCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tsacode)
                    .IsRequired()
                    .HasColumnName("TSACode")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Tsaname)
                    .IsRequired()
                    .HasColumnName("TSAName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany(p => p.TblTsasetup)
                    .HasPrincipalKey(p => p.AreaCode)
                    .HasForeignKey(d => d.AreaCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTSASetup_tblArea");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.TblTsasetup)
                    .HasPrincipalKey(p => p.RegionCode)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTSASetup_tblRegion");
            });

            modelBuilder.Entity<TblTsatoolReceiveAttachment>(entity =>
            {
                entity.ToTable("tblTSAToolReceiveAttachment");

                entity.HasIndex(e => e.AttachmentCode)
                    .HasName("IX_tblTSAToolReceiveAttachment")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.AttachmentCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AttachmentDate).HasColumnType("datetime");

                entity.Property(e => e.AttachmentTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasColumnName("EUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnName("IDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasColumnName("IUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalFileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tsacode)
                    .IsRequired()
                    .HasColumnName("TSACode")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.AttachmentTypeCodeNavigation)
                    .WithMany(p => p.TblTsatoolReceiveAttachment)
                    .HasPrincipalKey(p => p.AttachmentTypeCode)
                    .HasForeignKey(d => d.AttachmentTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTSAToolReceiveAttachment_tblAttachmentType");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblTsatoolReceiveAttachment>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTSAToolReceiveAttachment_tblTSAToolReceiveAttachment");

                entity.HasOne(d => d.TsacodeNavigation)
                    .WithMany(p => p.TblTsatoolReceiveAttachment)
                    .HasPrincipalKey(p => p.Tsacode)
                    .HasForeignKey(d => d.Tsacode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTSAToolReceiveAttachment_tblTSASetup");
            });
        }
    }
}
