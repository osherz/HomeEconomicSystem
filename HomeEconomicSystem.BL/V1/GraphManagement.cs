using HomeEconomicSystem.BE; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.Dal;
using HomeEconomicSystem.BL.V1.Extensions;
using System.Globalization;

namespace HomeEconomicSystem.BL.V1
{
    class GraphManagement : IGraphManagement
    {
        private IDb _db;
        private Validation _validation;
        public GraphManagement()
        {
            _db = new DalFactory().GetDb();
            _validation = new Validation();
        }

        #region AddGraph
        public void AddGraph(CategoryGraph categoryGraph)
        {
            if (!_validation.Validate(categoryGraph))
                throw (new ArgumentException());

            _db.CategoryGraphs.Add(categoryGraph);
            _db.SaveChanges();
        }

        public void AddGraph(ProductGraph productGraph)
        {
            if (!_validation.Validate(productGraph))
                throw (new ArgumentException());
            _db.ProductGraphs.Add(productGraph);
            _db.SaveChanges();
        }

        public void AddGraph(StoreGraph storeGraph)
        {
            if (!_validation.Validate(storeGraph))
                throw (new ArgumentException());
            _db.StoreGraphs.Add(storeGraph);
            _db.SaveChanges();
        }

        public void AddGraph(TransactionsGraph transactionsGraph)
        {
            if (!_validation.Validate(transactionsGraph))
                throw (new ArgumentException());
            _db.TransactionsGraphs.Add(transactionsGraph);
            _db.SaveChanges();
        }
        #endregion AddGraph

        #region GetGraphs
        public IEnumerable<BasicGraph> GetCategoryGraphs()
        {
            return _db.CategoryGraphs;
        }

        public IEnumerable<BasicGraph> GetProductGraphs()
        {
            return _db.ProductGraphs;
        }

        public IEnumerable<BasicGraph> GetStoreGraphs()
        {
            return _db.StoreGraphs;
        }

        public IEnumerable<BasicGraph> GetTransactionGraphs()
        {
            return _db.TransactionsGraphs;
        }

        #endregion

        #region DeleteGraph

        public void DeleteGraph(BasicGraph graph)
        {
            if (graph is CategoryGraph)
            {
                _db.CategoryGraphs.Remove(graph as CategoryGraph);
            }
            else if (graph is ProductGraph)
            {
                _db.ProductGraphs.Remove(graph as ProductGraph);
            }
            else if (graph is StoreGraph)
            {
                _db.StoreGraphs.Remove(graph as StoreGraph);
            }
            else if (graph is TransactionsGraph)
            {
                _db.TransactionsGraphs.Remove(graph as TransactionsGraph);
            }
            else
            {
                throw new SystemException();
            }
            _db.SaveChanges();
        }

        #endregion

        #region AnalyzeGraph

        private IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(BasicGraph graph, IEnumerable<ProductTransaction> productTransactions, Func<ProductTransaction,int> getKey)
        {
            (DateTime endDate, DateTime startDate) = GetEndAndStartDates(graph.EndDate.Value, graph.StartDate.Value, graph.PastTimeType.Value, graph.PastTimeAmount.Value);
            AmountOrCost amountOrCost = graph.AmountOrCost;
            TimeType aggregationTimeType = graph.AggregationTimeType;

            var dailyCategoryGroups =
                            from productTransaction in productTransactions
                            where productTransaction.Transaction.DateTime.InRange(startDate, endDate)
                            group productTransaction by getKey(productTransaction) into newGroup1
                            from newGroup2 in
                                    (from productTransaction in newGroup1
                                     group productTransaction by GetTimeType(productTransaction.Transaction.DateTime, aggregationTimeType))
                            group newGroup2 by newGroup1.Key;

            IEnumerable<int> xCollection = GetXcollectios(startDate, endDate, aggregationTimeType);
            return GetGroupsPoints(dailyCategoryGroups, amountOrCost, xCollection);
        }

        private (DateTime,DateTime) GetEndAndStartDates(DateTime endDate, DateTime startDate, TimeType pastTimeType, int pastTimeAmount)
        {
            if (startDate == null && endDate == null)
                return (AddPastTime(pastTimeType, pastTimeAmount), DateTime.Now);
            return (endDate, startDate);
        }

        private DateTime AddPastTime(TimeType PastTimeType, int pastTimeAmount)
        {
            pastTimeAmount = (-1)*(pastTimeAmount);
            switch (PastTimeType)
            {
                case TimeType.Day:
                    return DateTime.Now.AddDays(pastTimeAmount);
                    break;
                case TimeType.Week:
                    return DateTime.Now.AddDays(pastTimeAmount * 7);
                    break;
                case TimeType.Month:
                    return DateTime.Now.AddMonths(pastTimeAmount);
                    break;
                case TimeType.Year:
                    return DateTime.Now.AddYears(pastTimeAmount);
                    break;
                default:
                    throw new ArgumentException("TimeType not initialized properly");
                    break;
            }
        }

        private IEnumerable<int> GetXcollectios(DateTime StartDate, DateTime EndDate, TimeType aggregationTimeType)
        {
            int startKey = GetTimeType(StartDate, aggregationTimeType);
            int endKey = GetTimeType(EndDate, aggregationTimeType);
            while (startKey <= endKey)
            {
                yield return startKey;
                startKey += 1;
            }
        }

        private int GetTimeType(DateTime dateTime, TimeType timeType)
        {
            switch (timeType)
            {
                case TimeType.Day:
                    return dateTime.DayOfYear;
                    break;
                case TimeType.Week:
                    return dateTime.weekProjector();
                    break;
                case TimeType.Month:
                    return dateTime.Month;
                    break;
                case TimeType.Year:
                    return dateTime.Year;
                    break;
                default:
                    throw new ArgumentException("TimeType not initialized properly");
                    break;
            }
        }
        
        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(CategoryGraph categoryGraph)
        {
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(categoryGraph);

            return AnalyzeGraph(categoryGraph, productTransactions, pd => pd.Product.Category.Id); 
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(ProductGraph productGraph)
        {
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(productGraph);

            return AnalyzeGraph(productGraph, productTransactions, pd => pd.Product.Id);
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(StoreGraph storeGraph)
        {
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(storeGraph);

            return AnalyzeGraph(storeGraph, productTransactions, pd => pd.Store.Id);         
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(TransactionsGraph transactionsGraph)
        {
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(transactionsGraph);

            return AnalyzeGraph(transactionsGraph, productTransactions, pd => pd.Transaction.Id);
        }

        private Dictionary<int, IEnumerable<(double, double)>> GetGroupsPoints(IEnumerable<IGrouping<int, IGrouping<int,ProductTransaction>>> groupingGroups, AmountOrCost amountOrCost, IEnumerable<int> xCollection)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            foreach (var superGroup in groupingGroups)
            {
                Dictionary<double, double> points = new Dictionary<double, double>();
                xCollection.ToList().ForEach(x => points.Add(x, 0));
                foreach (var subGroup in superGroup)
                {
                    double temp = 0;
                    foreach (var producrTransaction in subGroup)
                    {
                        switch (amountOrCost)
                        {
                            case AmountOrCost.Amount:
                                temp += (producrTransaction.Amount);
                                break;
                            case AmountOrCost.Cost:
                                temp += (producrTransaction.UnitPrice * producrTransaction.Amount);
                                break;
                            default:
                                break;
                        }
                    }
                    points[subGroup.Key] = temp;
                }
                analyzeGraph.Add(superGroup.Key, points.Select(item => (item.Key, item.Value)));
            }
            return analyzeGraph;
        }
       
        private IEnumerable<ProductTransaction> GetProductTransactions(CategoryGraph categoryGraph)
        {
            return from category in categoryGraph.Categories
                   from product in category.Products
                   from productTransaction in product.ProductTransactions
                   select productTransaction;
        }

        private IEnumerable<ProductTransaction> GetProductTransactions(ProductGraph productGraph)
        {
            return 
                   from product in productGraph.Products
                   from productTransaction in product.ProductTransactions
                   select productTransaction;
        }

        private IEnumerable<ProductTransaction> GetProductTransactions(StoreGraph storeGraph)
        {
            return
                   from store in storeGraph.Stores
                   from productTransaction in store.ProductTransaction
                   select productTransaction;
        }

        private IEnumerable<ProductTransaction> GetProductTransactions(TransactionsGraph transactionsGraph)
        {
            IEnumerable<Transaction> transactions = _db.Transactions;
            return
                   from transaction in _db.Transactions
                   from productTransaction in transaction.ProductTransactions
                   select productTransaction;
        }

        #endregion

    }
}
