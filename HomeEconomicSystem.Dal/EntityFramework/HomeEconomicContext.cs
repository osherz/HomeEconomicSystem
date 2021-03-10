using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal.EntityFramework
{
    public class HomeEconomicContext : DbContext, IDb
    {
        public HomeEconomicContext() : base()
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.BarCode)
                .IsUnique();

            modelBuilder.Entity<Store>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<ProductGraph>()
                .HasMany<Product>(s => s.Products)
                .WithMany();

            modelBuilder.Entity<ProductGraph>()
                .HasIndex(p => p.Title)
                .IsUnique();

            modelBuilder.Entity<CategoryGraph>()
                .HasMany<Category>(s => s.Categories)
                .WithMany();

            modelBuilder.Entity<CategoryGraph>()
                .HasIndex(p => p.Title)
                .IsUnique();

            modelBuilder.Entity<StoreGraph>()
                .HasMany<Store>(s => s.Stores)
                .WithMany();

            modelBuilder.Entity<StoreGraph>()
               .HasIndex(p => p.Title)
               .IsUnique();

            modelBuilder.Entity<TransactionsGraph>()
               .HasIndex(p => p.Title)
               .IsUnique();
        }

        public DbSet<QRData> QRDatas { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<ProductTransaction> ProductTransactions { get; set; }

        public DbSet<StoreGraph> StoreGraphs { get; set; }

        public DbSet<ProductGraph> ProductGraphs { get; set; }

        public DbSet<CategoryGraph> CategoryGraphs { get; set; }

        public DbSet<TransactionsGraph> TransactionsGraphs { get; set; }



        #region Implement IDb

        IDbCollection<QRData> IDb.QRDatas => new DbSetCollection<QRData>(QRDatas);

        IDbCollection<Product> IDb.Products => new DbSetCollection<Product>(Products);

        IDbCollection<Category> IDb.Categories => new DbSetCollection<Category>(Categories);

        IDbCollection<Store> IDb.Stores => new DbSetCollection<Store>(Stores);

        IDbCollection<Transaction> IDb.Transactions => new DbSetCollection<Transaction>(Transactions);

        IDbCollection<ProductTransaction> IDb.ProductTransactions => new DbSetCollection<ProductTransaction>(ProductTransactions);

        IDbCollection<StoreGraph> IDb.StoreGraphs => new DbSetCollection<StoreGraph>(StoreGraphs);

        IDbCollection<ProductGraph> IDb.ProductGraphs => new DbSetCollection<ProductGraph>(ProductGraphs);

        IDbCollection<CategoryGraph> IDb.CategoryGraphs => new DbSetCollection<CategoryGraph>(CategoryGraphs);

        IDbCollection<TransactionsGraph> IDb.TransactionsGraphs => new DbSetCollection<TransactionsGraph>(TransactionsGraphs);

        void IDb.SaveChanges()
        {
            base.SaveChanges();
        }
        #endregion Implement IDb
    }
}
