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
        /// are initialized properly, if not returned "false".
        /// The category can be without products or Image.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Validate(Category category)
        {
            if (category.Id <= 0)
                return false;
            if (String.IsNullOrEmpty(category.Name))
                return false;
 //           if (String.IsNullOrEmpty(category.ImageFileName))
 //               return false;

            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "product"
        /// are initialized properly, if not returned "false".
        /// Product may not be associated with any category, transaction or
        /// Image.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Validate(Product product)
        {
            if (product.Id <= 0)
                return false;
            if (String.IsNullOrEmpty(product.BarCode))
                return false;
            if (String.IsNullOrEmpty(product.Name))
                return false;
            if (String.IsNullOrEmpty(product.Description))
                return false;
//            if (String.IsNullOrEmpty(product.ImageFileName))
//                return false;

            return true;
        }

        /// <summary>
        /// Checks if the relevant fields in the class "store"
        /// are initialized properly, if not returned "false".
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public bool Validate(Store store)
        {
            if (store.Id <= 0)
                return false;
            if (String.IsNullOrEmpty(store.Name))
                return false;
            if (String.IsNullOrEmpty(store.Address))
                return false;
            if (store.ProductTransaction == null || store.ProductTransaction.Count == 0)
                return false;

            return true;
        }
    }
}
