using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DA_Project
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model11")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Permission_Product> Permission_Product { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<TransferProduct> TransferProducts { get; set; }
        public virtual DbSet<TransferProductFrom> TransferProductFroms { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<Warehouse_Contains> Warehouse_Contains { get; set; }
        public virtual DbSet<Warehouse_Dispense> Warehouse_Dispense { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.Customer_Email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Customer_website)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Permission_Product)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Warehouse_Dispense)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductUnit>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Supplier_Email)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Supplier_website)
                .IsUnicode(false);

            modelBuilder.Entity<TransferProduct>()
                .HasMany(e => e.TransferProductFroms)
                .WithRequired(e => e.TransferProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Warehouse>()
                .Property(e => e.Warehouse_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Warehouse>()
                .Property(e => e.Manager_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Warehouse>()
                .Property(e => e.Warehouse_Location)
                .IsUnicode(false);

            modelBuilder.Entity<Warehouse>()
                .HasMany(e => e.TransferProducts)
                .WithOptional(e => e.Warehouse)
                .HasForeignKey(e => e.From_ID);

            modelBuilder.Entity<Warehouse>()
                .HasMany(e => e.TransferProducts1)
                .WithOptional(e => e.Warehouse1)
                .HasForeignKey(e => e.To_ID);

            modelBuilder.Entity<Warehouse_Contains>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<Warehouse_Contains>()
                .HasMany(e => e.Permission_Product)
                .WithRequired(e => e.Warehouse_Contains)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Warehouse_Contains>()
                .HasMany(e => e.TransferProductFroms)
                .WithRequired(e => e.Warehouse_Contains)
                .HasForeignKey(e => e.WarehouseContainsID_From)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Warehouse_Contains>()
                .HasMany(e => e.Warehouse_Dispense)
                .WithRequired(e => e.Warehouse_Contains)
                .HasForeignKey(e => e.WarehouseContainsID_From)
                .WillCascadeOnDelete(false);
        }
    }
}
