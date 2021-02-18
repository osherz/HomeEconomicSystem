﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    class ProductCatalogPageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public ProductCatalogPageDisplay()
        {
            Content = new View.ProductCatalogView();
            MenuItems = new List<MenuItem>();
        }
    }
}
