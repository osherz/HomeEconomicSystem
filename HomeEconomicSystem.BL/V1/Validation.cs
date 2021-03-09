using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL.V1
{
    class Validation
    {
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
            if (category.Id < 0)
                return false;
            if (String.IsNullOrEmpty(category.Name))
                return false;
           /* if (category.Products == null)
                return false;
            if (String.IsNullOrEmpty(category.ImageFileName))
                return false;*/

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
            if (product.Id < 0)
                return false;
            if (String.IsNullOrEmpty(product.BarCode))
                return false;
            if (String.IsNullOrEmpty(product.Name))
                return false;
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
            if (store.Id < 0)
                return false;
            if (String.IsNullOrEmpty(store.Name))
                return false;
            if (String.IsNullOrEmpty(store.Address))
                return false;

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
            if (categoryGraph.Categories == null || categoryGraph.Categories.Count <= 0)
                return false;
            return ValidateBaseProperty(categoryGraph);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "ProductGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="productGraph"></param>
        /// <returns></returns>
        public bool Validate(ProductGraph productGraph)
        {
            if (productGraph.Products == null || productGraph.Products.Count <= 0)
                return false;
            return ValidateBaseProperty(productGraph);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "StoreGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="storeGraph"></param>
        /// <returns></returns>
        public bool Validate(StoreGraph storeGraph)
        {
            if (storeGraph.Stores == null || storeGraph.Stores.Count <= 0)
                return false;
            return ValidateBaseProperty(storeGraph);
        }

        /// <summary>
        /// Checks if the relevant fields in the class "TransactionsGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="transactionsGraph"></param>
        /// <returns></returns>
        public bool Validate(TransactionsGraph transactionsGraph)
        {
            return ValidateBaseProperty(transactionsGraph);
        }


        /// <summary>
        /// Checks if the relevant fields in the class "BasicGraph"
        /// are initialized properly, if not return "false".
        /// </summary>
        /// <param name="basicGraph"></param>
        /// <returns></returns>
        public bool ValidateBaseProperty(BasicGraph basicGraph)
        {
            if (basicGraph.Id <= 0)
                return false;
            if (String.IsNullOrEmpty(basicGraph.Title))
                return false;
            if (basicGraph.StartDate == null || basicGraph.EndDate == null && basicGraph.PastTimeAmount == null || basicGraph.PastTimeType == null)
                return false;
            ///check if EndDate have a valid value
            if (DateTime.Compare(basicGraph.StartDate.Value, basicGraph.EndDate.Value) < 0)
                return false;
            if (basicGraph.PastTimeAmount < 0)
                return false;
            if (!basicGraph.PastTimeType.HasValue)
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
