using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    public interface IDataManagement
    {
        #region Category
        /// <summary>
        /// Add new category to DB.
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void AddCategory(Category category);

        /// <summary>
        /// Edit existing category.
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="KeyNotFoundException">Raise an exception if category id is not found.</exception>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void EditCategory(Category category);

        /// <summary>
        /// Delete existing category from DB.
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="KeyNotFoundException">Raise an exception if category id is not found.</exception>
        /// <exception cref="FailedBecauseNotEmptyException">Raise an exception if the category has products</exception>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void DeleteCategory(Category category);

        /// <summary>
        /// Get all categories that contains categoryName.
        /// If categoryName not assigned, return all categories.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        IEnumerable<Category> GetCategories(string categoryName = "");
        #endregion Category

        #region Product
        /// <summary>
        /// Edit existing product.
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="KeyNotFoundException">Raise an exception if product id is not found.</exception>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void EditProduct(Product product);

        /// <summary>
        /// Get all products that contains productName.
        /// If productName not assigned, return all products.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="categories">In which categories to search for products</param>
        /// <returns></returns>
        IEnumerable<Product> GetProducts(string productName = "", params Category[] categories);
        #endregion Product

        #region Store
        /// <summary>
        /// Edit existing store.
        /// </summary>
        /// <param name="store"></param>
        /// <exception cref="KeyNotFoundException">Raise an exception if store id is not found.</exception>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void EditStore(Store store);

        /// <summary>
        /// Get all stores that contains storeName.
        /// If storeName not assigned, return all stores.
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        IEnumerable<Store> GetStores(string storeName = "");
        #endregion Store

        #region Transaction
        /// <summary>
        /// Search for transactios that fit to the parameters.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="categories">If null, search in all categories</param>
        /// <param name="products">If null, search in all products</param>
        /// <param name="stores">If null, search in all stores</param>
        /// <returns></returns>
        IEnumerable<Transaction> GetTransactions(DateTime? startDate = null, DateTime? endDate = null, IEnumerable<Store> stores = null, IEnumerable<Product> products = null, IEnumerable<Category> categories = null);
        #endregion Transaction
    }
}
