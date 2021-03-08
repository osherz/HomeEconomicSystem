using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.Dal;
using System.Data.Entity;

namespace HomeEconomicSystem.BL.V1
{
    internal class DataManagement : IDataManagement
    {
        private IDb _db;
        private Validation _validation;

        public DataManagement()
        {
            _db = new DalFactory().GetDb();
            _validation = new Validation();
        }

        public void AddCategory(Category category)
        {
            if(_validation.Validate(category))
                throw (new ArgumentException("Category must have: Id, Name."));

            _db.Categories.Add(category);
            _db.SaveChanges();            
        }

        public void DeleteCategory(Category category)
        {
            if (_validation.Validate(category))
                throw (new ArgumentException("Category must have: Id, Name."));

            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        public void EditCategory(Category category)
        {
            if (!_validation.Validate(category))
                throw (new ArgumentException("Category must have: Id, Name."));

            Category oldCategory = _db.Categories.Single(c => c.Id == category.Id);
            oldCategory.ImageFileName = category.ImageFileName;
            oldCategory.Name = category.Name;
            _db.SaveChanges();
        }

        public void EditProduct(Product product)
        {
            if (!_validation.Validate(product))
                throw (new ArgumentException("Product must have: Id, Name, BarCode, Description."));

            Product oldProduct= _db.Products.Single(p => p.Id == product.Id);
            oldProduct.ImageFileName = product.ImageFileName;
            oldProduct.Name = product.Name;
            oldProduct.Category = product.Category;
            oldProduct.Description = product.Description;
            oldProduct.BarCode = product.BarCode;
            _db.SaveChanges();
        }

        public void EditStore(Store store)
        {
            if (_validation.Validate(store))
                throw (new ArgumentException("Store must have: Id, Name, Address, initialized List of ProductTransaction."));

            Store oldStore= _db.Stores.Single(p => p.Id == store.Id);
            oldStore.Address = store.Address;
            oldStore.Name = store.Name;
            _db.SaveChanges();
        }

        public IEnumerable<Category> GetCategories(string categoryName = "")
        {
            return _db.Categories.AsNoTracking().Where(c => c.Name.Contains(categoryName));
        }

        /// <summary>
        /// get Products by string which contained in the products names
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts(string productName = "", params Category[] categories)
        {
            return from product in _db.Products.AsNoTracking()
                   where product.Name.Contains(productName)
                   let categoriesIds = categories.Select(c => c.Id)
                   where categories.Count() <= 0 || categoriesIds.Contains(product.Category.Id)
                   select product;
        }

        public IEnumerable<Store> GetStores(string storeName = "")
        {
            return _db.Stores.AsNoTracking().Where(s => s.Name.Contains(storeName));
        }

        public IEnumerable<Transaction> GetTransactions(DateTime? startDate = null, DateTime? endDate = null, IEnumerable<Store> stores = null, IEnumerable<Product> products = null, IEnumerable<Category> categories = null)
        {
            return _db.Transactions.AsNoTracking();
        }
    }
}
