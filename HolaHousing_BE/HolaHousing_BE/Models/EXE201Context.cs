using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HolaHousing_BE.Models
{
    public partial class EXE201Context : DbContext
    {
        public EXE201Context()
        {
        }

        public EXE201Context(DbContextOptions<EXE201Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Amentity> Amentities { get; set; } = null!;
        public virtual DbSet<New> News { get; set; } = null!;
        public virtual DbSet<PartContent> PartContents { get; set; } = null!;
        public virtual DbSet<PostPrice> PostPrices { get; set; } = null!;
        public virtual DbSet<PostType> PostTypes { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =localhost; database = EXE201;uid=sa;pwd=123;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amentity>(entity =>
            {
                entity.ToTable("Amentity");

                entity.Property(e => e.AmentityId).HasColumnName("Amentity_ID");

                entity.Property(e => e.Amentity_Name)
                    .HasMaxLength(50)
                    .HasColumnName("Amentity");

                entity.HasMany(d => d.Properties)
                    .WithMany(p => p.Amentities)
                    .UsingEntity<Dictionary<string, object>>(
                        "AmentityProperty",
                        l => l.HasOne<Property>().WithMany().HasForeignKey("PropertyId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Amentity___Prope__4316F928"),
                        r => r.HasOne<Amentity>().WithMany().HasForeignKey("AmentityId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Amentity___Ament__4222D4EF"),
                        j =>
                        {
                            j.HasKey("AmentityId", "PropertyId").HasName("PK__Amentity__BD991E969413D791");

                            j.ToTable("Amentity_Property");

                            j.IndexerProperty<int>("AmentityId").HasColumnName("Amentity_ID");

                            j.IndexerProperty<int>("PropertyId").HasColumnName("Property_ID");
                        });
            });

            modelBuilder.Entity<New>(entity =>
            {
                entity.ToTable("New");

                entity.Property(e => e.NewId).HasColumnName("New_ID");

                entity.Property(e => e.Author).HasMaxLength(100);

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.PostDate)
                    .HasColumnType("date")
                    .HasColumnName("Post_Date");

                entity.Property(e => e.Sumary).HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__New__Created_By__36B12243");
            });

            modelBuilder.Entity<PartContent>(entity =>
            {
                entity.ToTable("Part_Content");

                entity.Property(e => e.PartContentId).HasColumnName("Part_Content_ID");

                entity.Property(e => e.Alt).HasMaxLength(100);

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.NewId).HasColumnName("New_ID");

                entity.Property(e => e.Src).HasMaxLength(200);

                entity.HasOne(d => d.New)
                    .WithMany(p => p.PartContents)
                    .HasForeignKey(d => d.NewId)
                    .HasConstraintName("FK__Part_Cont__New_I__398D8EEE");
            });

            modelBuilder.Entity<PostPrice>(entity =>
            {
                entity.ToTable("Post_Price");

                entity.Property(e => e.PostPriceId).HasColumnName("Post_Price_ID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.TypeId).HasColumnName("Type_ID");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.PostPrices)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Post_Pric__Type___2B3F6F97");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Post_Typ__FE90DDFE49B96570");

                entity.ToTable("Post_Type");

                entity.Property(e => e.TypeId).HasColumnName("Type_ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.TypeName)
                    .HasMaxLength(20)
                    .HasColumnName("Type_Name");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property");

                entity.Property(e => e.PropertyId).HasColumnName("Property_ID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Content).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Owner).HasMaxLength(50);

                entity.Property(e => e.PhoneNum)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PostTime).HasColumnType("datetime");

                entity.Property(e => e.PosterId).HasColumnName("Poster_ID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.PropertyType)
                    .HasMaxLength(20)
                    .HasColumnName("Property_Type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.Property(e => e.Ward).HasMaxLength(50);

                entity.HasOne(d => d.Poster)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.PosterId)
                    .HasConstraintName("FK__Property__Poster__2E1BDC42");

                entity.HasMany(d => d.PostPrices)
                    .WithMany(p => p.Properties)
                    .UsingEntity<Dictionary<string, object>>(
                        "PropertyPostPrice",
                        l => l.HasOne<PostPrice>().WithMany().HasForeignKey("PostPriceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Property___Post___46E78A0C"),
                        r => r.HasOne<Property>().WithMany().HasForeignKey("PropertyId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Property___Prope__45F365D3"),
                        j =>
                        {
                            j.HasKey("PropertyId", "PostPriceId").HasName("PK__Property__1C6DB2D643208711");

                            j.ToTable("Property_PostPrice");

                            j.IndexerProperty<int>("PropertyId").HasColumnName("Property_ID");

                            j.IndexerProperty<int>("PostPriceId").HasColumnName("Post_Price_ID");
                        });
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => new { e.PropertyId, e.Image })
                    .HasName("PK__Property__747EA80B84027BBA");

                entity.ToTable("Property_Image");

                entity.Property(e => e.PropertyId).HasColumnName("Property_ID");

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyImages)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Property___Prope__31EC6D26");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("Tag_ID");

                entity.Property(e => e.TagName)
                    .HasMaxLength(50)
                    .HasColumnName("Tag_Name");

                entity.HasMany(d => d.News)
                    .WithMany(p => p.Tags)
                    .UsingEntity<Dictionary<string, object>>(
                        "NewTag",
                        l => l.HasOne<New>().WithMany().HasForeignKey("NewId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__New_Tag__New_ID__3D5E1FD2"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__New_Tag__Tag_ID__3C69FB99"),
                        j =>
                        {
                            j.HasKey("TagId", "NewId").HasName("PK__New_Tag__2A0D5FD69BCF2182");

                            j.ToTable("New_Tag");

                            j.IndexerProperty<int>("TagId").HasColumnName("Tag_ID");

                            j.IndexerProperty<int>("NewId").HasColumnName("New_ID");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNum)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__Role_ID__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
