﻿using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace HomeEconomicSystem.PL.Model
{
    public class ProductsModel
    {
        IDataManagement _dataMenegement;
        // TODO: Find a way to use a read-only ObservableCollection
        public ObservableCollection<Product> ProductsList { get; private set; }
        public ProductsModel()
        {
            _dataMenegement = new BL.BL().DataManagement;
            ProductsList = new ObservableCollection<Product>();
            Filter();
        }

        public void AddProduct(Product product)
        {
            _dataMenegement.AddProduct(product);
        }

        public void Update(Product product)
        {
            _dataMenegement.EditProduct(product);
        }

        public void Filter(string name = "", params Category[] categories)
        {
            ProductsList.Clear();
            _dataMenegement.GetProducts(name, categories).ForEach(p => ProductsList.Add(p));
        }
    }
}
