using HomeEconomicSystem.BE;
using HomeEconomicSystem.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL.V1
{
    public class Validation : IValidation
    {
        private IDb _db;
        //private GraphManagement graphManagement;
        public Validation()
        {
            _db = new DalFactory().GetDb();
        }
        /// <summary>
        /// Checks if the relevant fields in the class "category"
        /// are initialized properly, 
        /// if not return "false".
        /// The category can be without products or Image.
        /// The category can have 0 products.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Validate(Category category)
        {
            var dupliCategory =
                from category1 in _db.Categories
                where category1.Name == category.Name 
                        && category1.Id != category.Id
                select category1;
            if (dupliCategory.Any())
                return false;


            if (category.Id < 0)
                return false;
            if (string.IsNullOrEmpty(category.Name))
                return false;


            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "product"
        /// are initialized properly, if not return "false".
        /// Product may not be associated with any category, transaction or
        /// Image.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Validate(Product product)
        {
            var dupliProduct =
                from product1 in _db.Products
                where product1.Id != product.Id &&
                    (product1.BarCode == product.BarCode || product1.Name == product.Name)
                select product1;
            if (dupliProduct.Any())
                return false;

            if (product.Id < 0)
                return false;
            if (string.IsNullOrEmpty(product.BarCode))
                return false;
            if (string.IsNullOrEmpty(product.Name))
                return false;
            if (!Validate(product.Category)) return false;
            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "store"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public bool Validate(Store store)
        {
            var dupliStore =
                from store1 in _db.Stores
                where store1.Id != store.Id && 
                        store1.Name == store.Name
                select store1;
            if (dupliStore.Any())
                return false;

            if (store.Id < 0)
                return false;
            if (string.IsNullOrEmpty(store.Name))
                return false;

            if (string.IsNullOrEmpty(store.Address))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "TransactionsGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="transactionsGraph"></param>
        /// <returns></returns>
        public bool Validate(Transaction transaction)
        {
            if( transaction.ProductTransactions is null ||
                transaction.ProductTransactions.Count == 0)
            {
                return false;
            }

            foreach(var pt in transaction.ProductTransactions)
            {
                if (!Validate(pt)) return false;
            }

            return true;
        }

        private bool Validate(ProductTransaction pt)
        {
            if (pt.Product is null || !Validate(pt.Product)) return false;
            if (pt.Store is null || !Validate(pt.Store)) return false;
            if (pt.UnitPrice <= 0 || pt.Amount <= 0) return false;
            
            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "CategoryGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="categoryGraph"></param>
        /// <returns></returns>
        public bool Validate(CategoryGraph categoryGraph)
        {
            if (categoryGraph.Categories == null || categoryGraph.Categories.Count < 0)
                return false;
            return ValidateBaseProperty(categoryGraph, _db.CategoryGraphs);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "ProductGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="productGraph"></param>
        /// <returns></returns>
        public bool Validate(ProductGraph productGraph)
        {
            if (productGraph.Products == null || productGraph.Products.Count < 0)
                return false;
            return ValidateBaseProperty(productGraph, _db.ProductGraphs);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "StoreGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="storeGraph"></param>
        /// <returns></returns>
        public bool Validate(StoreGraph storeGraph)
        {
            if (storeGraph.Stores == null || storeGraph.Stores.Count < 0)
                return false;
            return ValidateBaseProperty(storeGraph, _db.StoreGraphs);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "TransactionsGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="transactionsGraph"></param>
        /// <returns></returns>
        public bool Validate(TransactionsGraph transactionsGraph)
        {
            return ValidateBaseProperty(transactionsGraph, _db.TransactionsGraphs);
        }


        /// <summary>
        /// Checks if the relevant fields in the class "BasicGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="basicGraph"></param>
        /// <returns></returns>
        public bool ValidateBaseProperty(BasicGraph basicGraph, IEnumerable<BasicGraph> graphs)
        {
            var dupliGraph =
                from graph in graphs
                where graph.Title == basicGraph.Title && graph.Id != basicGraph.Id
                select graph;
            if (dupliGraph.Any())
                return false;

            if (basicGraph.Id < 0)
                return false;

            if ((basicGraph.StartDate == null || basicGraph.EndDate == null) && (basicGraph.PastTimeAmount == null || basicGraph.PastTimeType == null))
                return false;
            if (string.IsNullOrEmpty(basicGraph.Title))
                return false;
            if (basicGraph.StartDate == null && basicGraph.EndDate == null)
                if (basicGraph.PastTimeAmount == null || basicGraph.PastTimeType == null || basicGraph.PastTimeAmount < 0 || !basicGraph.PastTimeType.HasValue)
                    return false;
            if (basicGraph.PastTimeAmount == null && basicGraph.PastTimeType == null)
                if (basicGraph.StartDate == null || basicGraph.EndDate == null)
                    return false;

            ///check if EndDate have a valid value
            if (DateTime.Compare(basicGraph.StartDate.Value, basicGraph.EndDate.Value) > 0)
                return false;

            ///check if GraphType have a valid value
            if (!Enum.IsDefined(typeof(GraphType), basicGraph.GraphType))
                return false;
            ///check if AmountOrCost have a valid value
            if (!Enum.IsDefined(typeof(AmountOrCost), basicGraph.AmountOrCost))
                return false;
            //TODO: create specipice exeption for every check (instad of returns false)
            return true;
        }

    }
}
