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

        public DataManagement()
        {
            _db = new DalFactory().GetDb();
        }

        public void AddCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        public void EditCategory(Category category)
        {
            Category oldCategory = _db.Categories.Single(c => c.Id == category.Id);
            oldCategory.ImageFileName = category.ImageFileName;
            oldCategory.Name = category.Name;
            _db.SaveChanges();
        }

        public void EditProduct(Product product)
        {
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
            Store oldStore= _db.Stores.Single(p => p.Id == store.Id);
            oldStore.Address = store.Address;
            oldStore.Name = store.Name;
            _db.SaveChanges();
        }

        public IEnumerable<Category> GetCategories(string categoryName = "")
        {
            return _db.Categories.AsNoTracking().Where(c => c.Name.Contains(categoryName));
        }

        public IEnumerable<Product> GetProducts(string productName = "", params Category[] categories)
        {
            return from product in _db.Products
                   where product.Name.Contains(productName)
                   let categoriesIds = categories.Select(c => c.Id)
                   where categories.Count() <= 0 || categoriesIds.Contains(product.Category.Id)
                   select product;
        }

        public IEnumerable<Store> GetStores(string storeName = "")
        {
            return _db.Stores.Where(s => s.Name.Contains(storeName));
        }

        public IEnumerable<Transaction> GetTransactions(DateTime startDate, DateTime endDate, IEnumerable<Store> stores = null, IEnumerable<Product> products = null, IEnumerable<Category> categories = null)
        {
            return _db.Transactions;
        }
    }
}
