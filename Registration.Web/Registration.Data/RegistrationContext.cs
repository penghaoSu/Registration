using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Registration.Data
{
    public partial class RegistrationContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityArea> CityArea { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<LogFile> LogFile { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<ProductKey> ProductKey { get; set; }

        public RegistrationContext(DbContextOptions<RegistrationContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<CityArea>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnType("char(5)");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CityArea)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityArea_City");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Contact).HasMaxLength(10);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Number).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.CityArea)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CityAreaId)
                    .HasConstraintName("FK_Customer_CityArea");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Customer_City");
            });

            modelBuilder.Entity<LogFile>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogFile)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LogFile_AspNetUsers");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.Salesperson).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Deliveryperson).HasMaxLength(50);

                entity.Property(e => e.LeaseDateEnd)
                    .HasColumnName("leaseDateEnd")
                    .HasColumnType("date");

                entity.Property(e => e.LeaseDateStr)
                    .HasColumnName("leaseDateStr")
                    .HasColumnType("date");

                entity.Property(e => e.Remark).HasColumnType("text");

                entity.Property(e => e.Revision).HasMaxLength(50);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.Property(e => e.WarrantyPeriodEnd).HasColumnType("date");

                entity.Property(e => e.WarrantyPeriodStr).HasColumnType("date");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<ProductKey>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.LicenceKey)
                    .IsRequired()
                    .HasColumnName("LicenceKey")
                    .HasMaxLength(50);

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.ProductKey)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductKey_OrderDetail");
            });
        }
    }
}
