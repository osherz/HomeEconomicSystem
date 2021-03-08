using HomeEconomicSystem.BE; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.Dal;

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

        private IEnumerable<(double, double)> GetPoints()
        {
            var pointRand = new Random((int)DateTime.Now.Ticks);
            var points = new (double, double)[4];
            for (int i = 0; i < points.Length; i++)
            {
                points[i]=(i, pointRand.Next(0, 10));
            }
            return points;
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(CategoryGraph categoryGraph)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            foreach (var item in categoryGraph.Categories)
            {
                analyzeGraph.Add(item.Id, GetPoints());
            }
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(ProductGraph productGraph)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            foreach (var item in productGraph.Products)
            {
                analyzeGraph.Add(item.Id, GetPoints());
            }
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(StoreGraph storeGraph)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            foreach (var item in storeGraph.Stores)
            {
                analyzeGraph.Add(item.Id, GetPoints());
            }
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(double, double)>> AnalyzeGraph(TransactionsGraph transactionsGraph)
        {
            Dictionary<int, IEnumerable<(double, double)>> analyzeGraph = new Dictionary<int, IEnumerable<(double, double)>>();
            analyzeGraph.Add(1, GetPoints());
            return analyzeGraph;
        }

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
    }
}
