﻿using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GraphPanelUC.xaml
    /// </summary>
    public partial class GraphPanelUC : UserControl
    {


        public ObservableCollection<BasicGraph> GraphsCollection
        {
            get { return (ObservableCollection<BasicGraph>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("GraphsCollection", typeof(ObservableCollection<BasicGraph>), typeof(GraphPanelUC), new PropertyMetadata(null));


        public GraphPanelUC()
        {
            InitializeComponent();
        }
    }
}
