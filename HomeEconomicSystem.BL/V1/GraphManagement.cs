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
        public GraphManagement()
        {
            _db = new DalFactory().GetDb();
        }
        public void AddGraph(CategoryGraph categoryGraph)
        {
            _db.CategoryGraphs.Add(categoryGraph);
            _db.SaveChanges();
        }

        public void AddGraph(ProductGraph productGraph)
        {
            _db.ProductGraphs.Add(productGraph);
            _db.SaveChanges();
        }

        public void AddGraph(StoreGraph storeGraph)
        {
            _db.StoreGraphs.Add(storeGraph);
            _db.SaveChanges();
        }

        public void AddGraph(TransactionsGraph transactionsGraph)
        {
            _db.TransactionsGraphs.Add(transactionsGraph);
            _db.SaveChanges();
        }

        public IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(CategoryGraph categoryGraph)
        {
            Dictionary<int, IEnumerable<(int, int)>> analyzeGraph = new Dictionary<int, IEnumerable<(int, int)>>();
            analyzeGraph.Add(1, new[] {(0,0),(1,1) } );
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(ProductGraph productGraph)
        {
            Dictionary<int, IEnumerable<(int, int)>> analyzeGraph = new Dictionary<int, IEnumerable<(int, int)>>();
            analyzeGraph.Add(1, new[] { (0, 0), (1, 1) });
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(StoreGraph storeGraph)
        {
            Dictionary<int, IEnumerable<(int, int)>> analyzeGraph = new Dictionary<int, IEnumerable<(int, int)>>();
            analyzeGraph.Add(1, new[] { (0, 0), (1, 1) });
            return analyzeGraph;
        }

        public IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(TransactionsGraph transactionsGraph)
        {
            Dictionary<int, IEnumerable<(int, int)>> analyzeGraph = new Dictionary<int, IEnumerable<(int, int)>>();
            analyzeGraph.Add(1, new[] { (0, 0), (1, 1) });
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
