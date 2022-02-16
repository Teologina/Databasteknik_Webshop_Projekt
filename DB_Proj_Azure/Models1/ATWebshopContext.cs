using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DB_Proj_Azure.Models1
{
    public partial class ATWebshopContext : DbContext
    {
        public ATWebshopContext()
        {
        }

        public ATWebshopContext(DbContextOptions<ATWebshopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ShoppingCartTest> ShoppingCartTests { get; set; }
        public virtual DbSet<Shoptest> Shoptests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Categories)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(30);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CustomerAddress).HasMaxLength(50);

                entity.Property(e => e.CustomerCity).HasMaxLength(50);

                entity.Property(e => e.CustomerPostalCode).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetails");

                //entity.HasOne(d => d.Product)
                //    .WithMany(p => p.OrderDetails)
                //    .HasForeignKey(d => d.ProductId)
                //    .HasConstraintName("FK_OrderDetailsSecond");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductInfo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoriesId)
                    .HasConstraintName("FK_Products");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ShoppingCart");
            });

            modelBuilder.Entity<ShoppingCartTest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ShoppingCartTest");
            });

            modelBuilder.Entity<Shoptest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("shoptest");

                entity.HasIndex(e => e.Id, "UQ__shoptest__3213E83E865C9585")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Prodid).HasColumnName("prodid");

                entity.Property(e => e.Quant).HasColumnName("quant");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
