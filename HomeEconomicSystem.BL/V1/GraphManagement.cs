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
        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(CategoryGraph categoryGraph)
        {


            IEnumerable<(double, double)> points = new List<(double, double)>();
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(categoryGraph);

            ICollection<Category> categories = categoryGraph.Categories;
            DateTime StartDate = (DateTime)categoryGraph.StartDate;
            DateTime EndDate = (DateTime)categoryGraph.StartDate;
            AmountOrCost amountOrCost = categoryGraph.AmountOrCost;
            TimeType AggregationTimeType = categoryGraph.AggregationTimeType;

            switch (AggregationTimeType)
            {
                case TimeType.Day:
                    var dailyCategoryGroups =
                            from productTransaction1 in productTransactions
                            where productTransaction1.Transaction.DateTime.InRange(StartDate, EndDate)
                            group productTransaction1 by productTransaction1.Product.Category.Id into newGroup1
                            from newGroup2 in
                                    (from productTransaction1 in newGroup1
                                     group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                            group newGroup2 by newGroup1.Key;                    
                    return GetGroupsPoints(dailyCategoryGroups, amountOrCost);
                    break;
                case TimeType.Week:
                    var weeklyCategoryGroups =
                            from productTransaction1 in productTransactions
                            where productTransaction1.Transaction.DateTime.InRange(StartDate, EndDate)
                            group productTransaction1 by productTransaction1.Product.Category.Id into newGroup1
                            from newGroup2 in
                                    (from productTransaction1 in newGroup1
                                     group productTransaction1 by productTransaction1.Transaction.DateTime.weekProjector())
                            group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(weeklyCategoryGroups, amountOrCost);
                    break;
                case TimeType.Month:
                    var monthlyCategoryGroups =
                                                from productTransaction1 in productTransactions
                                                where productTransaction1.Transaction.DateTime.InRange(StartDate, EndDate)
                                                group productTransaction1 by productTransaction1.Product.Category.Id into newGroup1
                                                from newGroup2 in
                                                        (from productTransaction1 in newGroup1
                                                         group productTransaction1 by productTransaction1.Transaction.DateTime.Month)
                                                group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(monthlyCategoryGroups, amountOrCost); 
                    break;
                case TimeType.Year:
                    var yearlyCategoryGroups =
                                                from productTransaction1 in productTransactions
                                                where productTransaction1.Transaction.DateTime.InRange(StartDate, EndDate)
                                                group productTransaction1 by productTransaction1.Product.Category.Id into newGroup1
                                                from newGroup2 in
                                                        (from productTransaction1 in newGroup1
                                                         group productTransaction1 by productTransaction1.Transaction.DateTime.Year)
                                                group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(yearlyCategoryGroups, amountOrCost);
                    break;
                default:
                    throw new ArgumentException("AggregationTimeType not set properly");
                    break;
            }
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(ProductGraph productGraph)
        {
            IEnumerable<(double, double)> points = new List<(double, double)>();
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(productGraph);

            DateTime StartDate = (DateTime)productGraph.StartDate;
            DateTime EndDate = (DateTime)productGraph.StartDate;
            AmountOrCost amountOrCost = productGraph.AmountOrCost;
            TimeType AggregationTimeType = productGraph.AggregationTimeType;


            switch (AggregationTimeType)
            {
                case TimeType.Day:
                    var dailyProductGroups =
                            from productTransaction in productTransactions
                            where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                            group productTransaction by productTransaction.Product.Id into newGroup1
                            from newGroup2 in
                                    (from productTransaction1 in newGroup1
                                     group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                            group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(dailyProductGroups, amountOrCost);
                    break;
                case TimeType.Week:
                    var weeklyProductGroups =
                            from productTransaction in productTransactions
                            where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                            group productTransaction by productTransaction.Product.Id into newGroup1
                            from newGroup2 in
                                    (from productTransaction1 in newGroup1
                                     group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                            group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(weeklyProductGroups, amountOrCost);
                    break;
                case TimeType.Month:
                    var monthlyProductGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Product.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(monthlyProductGroups, amountOrCost);
                    break;
                case TimeType.Year:
                    var yearlyProductGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Product.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(yearlyProductGroups, amountOrCost);
                    break;
                default:
                    throw new ArgumentException("AggregationTimeType not set properly");
                    break;
            }
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(StoreGraph storeGraph)
        {
            IEnumerable<(double, double)> points = new List<(double, double)>();
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(storeGraph);

            DateTime StartDate = (DateTime)storeGraph.StartDate;
            DateTime EndDate = (DateTime)storeGraph.StartDate;
            AmountOrCost amountOrCost = storeGraph.AmountOrCost;
            TimeType AggregationTimeType = storeGraph.AggregationTimeType;


            switch (AggregationTimeType)
            {
                case TimeType.Day:
                    var dailyStoreGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Store.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(dailyStoreGroups, amountOrCost);
                    break;
                case TimeType.Week:
                    var weeklyStoreGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Store.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.weekProjector())
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(weeklyStoreGroups, amountOrCost);
                    break;
                case TimeType.Month:
                    var monthlyStoreGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Store.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.Month)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(monthlyStoreGroups, amountOrCost);
                    break;
                case TimeType.Year:
                    var yearlyStoreGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Store.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.Year)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(yearlyStoreGroups, amountOrCost);
                    break;
                default:
                    throw new ArgumentException("AggregationTimeType not set properly");
                    break;
            }           
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(TransactionsGraph transactionsGraph)
        {
            IEnumerable<(double, double)> points = new List<(double, double)>();
            IEnumerable<ProductTransaction> productTransactions = GetProductTransactions(transactionsGraph);

            DateTime StartDate = (DateTime)transactionsGraph.StartDate;
            DateTime EndDate = (DateTime)transactionsGraph.StartDate;
            AmountOrCost amountOrCost = transactionsGraph.AmountOrCost;
            TimeType AggregationTimeType = transactionsGraph.AggregationTimeType;

            switch (AggregationTimeType)
            {
                case TimeType.Day:
                    var dailyTransactionGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Transaction.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.DayOfYear)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(dailyTransactionGroups, amountOrCost);
                    break;
                case TimeType.Week:
                    var weeklyTransactionGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Transaction.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.weekProjector())
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(weeklyTransactionGroups, amountOrCost);
                    break;
                case TimeType.Month:
                    var monthlyTransactionGroups =
                           from productTransaction in productTransactions
                           where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                           group productTransaction by productTransaction.Transaction.Id into newGroup1
                           from newGroup2 in
                                   (from productTransaction1 in newGroup1
                                    group productTransaction1 by productTransaction1.Transaction.DateTime.Month)
                           group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(monthlyTransactionGroups, amountOrCost);
                    break;
                case TimeType.Year:
                    var yearlyTransactionGroups =
                            from productTransaction in productTransactions
                            where productTransaction.Transaction.DateTime.InRange(StartDate, EndDate)
                            group productTransaction by productTransaction.Transaction.Id into newGroup1
                            from newGroup2 in
                                    (from productTransaction1 in newGroup1
                                     group productTransaction1 by productTransaction1.Transaction.DateTime.Year)
                            group newGroup2 by newGroup1.Key;
                    return GetGroupsPoints(yearlyTransactionGroups, amountOrCost);
                    break;
                default:
                    throw new ArgumentException("AggregationTimeType not set properly");
                    break;
            }
        }

        private Dictionary<int, IEnumerable<(double, double)>> GetGroupsPoints(IEnumerable<IGrouping<int, IGrouping<int,ProductTransaction>>> groupingGroups, AmountOrCost amountOrCost)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            switch (amountOrCost)
            {
                case AmountOrCost.Amount:
                    foreach (var superGroup in groupingGroups)
                    {
                        IEnumerable<(double, double)> points = new List<(double, double)>();
                        foreach (var subGroup in superGroup)
                        {
                            double amount = 0;
                            foreach (var producrTransaction in subGroup)
                            {
                                amount += (producrTransaction.Amount);
                            }
                            points.ToList().Add((subGroup.Key, amount));
                        }
                        analyzeGraph.Add(superGroup.Key, points);
                    }
                    break;
                case AmountOrCost.Cost:
                    foreach (var superGroup in groupingGroups)
                    {
                        IEnumerable<(double, double)> points = new List<(double, double)>();
                        foreach (var subGroup in superGroup)
                        {
                            double cost = 0;
                            foreach (var producrTransaction in subGroup)
                            {
                                cost += (producrTransaction.UnitPrice * producrTransaction.Amount);
                            }
                            points.ToList().Add((subGroup.Key, cost));
                        }
                        analyzeGraph.Add(superGroup.Key, points);
                    }
                    break;
                default:
                    throw new ArgumentException("AmountOrCost not set properly");
                    break;
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
