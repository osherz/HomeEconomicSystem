using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    public interface IDb: IDisposable
    {
        IDbCollection<QRData> QRDatas { get; }
        IDbCollection<Product> Products { get;}
        IDbCollection<Category> Categories { get; }
        IDbCollection<Store> Stores { get; }
        IDbCollection<Transaction> Transactions { get; }
        IDbCollection<ProductTransaction> ProductTransactions { get; }
        IDbCollection<StoreGraph> StoreGraphs { get; }
        IDbCollection<ProductGraph> ProductGraphs { get; }
        IDbCollection<CategoryGraph> CategoryGraphs { get; }
        IDbCollection<TransactionsGraph> TransactionsGraphs { get; }


        void SaveChanges();
    }
}
