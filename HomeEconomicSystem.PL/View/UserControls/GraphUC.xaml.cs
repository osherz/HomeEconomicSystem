﻿using HomeEconomicSystem.BE;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeEconomicSystem.PL.View.UserControls
{
    /// <summary>
    /// Interaction logic for GraphUC.xaml
    /// </summary>
    public partial class GraphUC : UserControl
    {


        public GraphData Graph
        {
            get { return (GraphData)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Graph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(GraphData), typeof(GraphUC), new PropertyMetadata(null));

        public IReadOnlyList<ViewModel.MenuItem> MenuItems
        {
            get { return (IReadOnlyList<ViewModel.MenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(IReadOnlyList<ViewModel.MenuItem>), typeof(GraphUC), new PropertyMetadata(null));

        public GraphUC()
        {
            InitializeComponent();

            MenuItems = new List<ViewModel.MenuItem>(new[]
  {
                new ViewModel.MenuItem("ערוך", PackIconKind.Edit,null),
                new ViewModel.MenuItem("מחק", PackIconKind.Delete,null)
            });
        }
    }
}
