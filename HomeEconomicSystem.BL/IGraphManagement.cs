using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.BE;

namespace HomeEconomicSystem.BL
{
    public interface IGraphManagement
    {
        #region Add funtions
        /// <summary>
        /// Add new categoryGraph to DB.
        /// </summary>
        /// <param name="categoryGraph"></param>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>

        void AddGraph(CategoryGraph categoryGraph);

        /// <summary>
        /// Add new productGraph to DB.
        /// </summary>
        /// <param name="productGraph"></param>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>

        void AddGraph(ProductGraph productGraph);

        /// <summary>
        /// Add new storeGraph to DB.
        /// </summary>
        /// <param name="storeGraph"></param>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void AddGraph(StoreGraph storeGraph);

        /// <summary>
        /// Add new transactionsGraph to DB.
        /// </summary>
        /// <param name="transactionsGraph"></param>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void AddGraph(TransactionsGraph transactionsGraph);
        #endregion Add function

        #region Anlyze function
        /// <summary>
        /// Anlyze the required type of graph into dictionary where the key is the 
        /// category ID  and the value is list of coordinates. 
        /// </summary>
        /// <param name="categoryGraph"></param>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(CategoryGraph categoryGraph);

        /// <summary>
        /// Anlyze the required type of graph into dictionary where the key is the 
        /// Product ID  and the value is list of coordinates. 
        /// </summary>
        /// <param name="productGraph"></param>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(ProductGraph productGraph);

        /// <summary>
        /// Anlyze the required type of graph into dictionary where the key is the 
        /// store ID  and the value is list of coordinates. 
        /// </summary>
        /// <param name="storeGraph"></param>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(StoreGraph storeGraph);

        /// <summary>
        /// Anlyze the required type of graph into dictionary where the key is the 
        /// transaction ID  and the value is list of coordinates. 
        /// </summary>
        /// <param name="transactionsGraph"></param>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IReadOnlyDictionary<int, IEnumerable<(int, int)>> AnalyzeGraph(TransactionsGraph transactionsGraph);

        #endregion Analyze function

        #region Get function

        /// <summary>
        /// return all category graph from DB.
        /// </summary>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IEnumerable<BasicGraph> GetCategoryGraphs();

        /// <summary>
        /// return all product graph from DB.
        /// </summary>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IEnumerable<BasicGraph> GetProductGraphs();

        /// <summary>
        /// return all store graph from DB.
        /// </summary>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IEnumerable<BasicGraph> GetStoreGraphs();

        /// <summary>
        /// return all transaction graph from DB.
        /// </summary>
        /// <returns></returns>
        ///<exception cref="SystemException">Raise an exception if action failed.</exception>
        IEnumerable<BasicGraph> GetTransactionGraphs();

        #endregion Get function
    }
}
