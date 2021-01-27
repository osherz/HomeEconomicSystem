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
        internal HomeEconomicContext() : base()
        {
        }

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
