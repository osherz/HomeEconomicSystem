using Graphviz4Net.Graphs;
using HomeEconomicSystem.BE;
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

namespace HomeEconomicSystem.PL.View
{
    /// <summary>
    /// Interaction logic for AssociationRulesView.xaml
    /// </summary>
    public partial class AssociationRulesView : UserControl
    {
        public Graph<Product> Graph { get; set; }

        public AssociationRulesView()
        {
            Graph = new Graph<Product>();
            var a = new Product { Name = "Jonh", ImageFileName = @"C:\Users\osher\Pictures\WhatsApp Image 2020-12-16 at 12.45.10.jpeg" };
            var b = new Product { Name = "Michael", ImageFileName = @"C:\Users\osher\Pictures\WhatsApp Image 2020-12-16 at 12.45.10.jpeg" };
            Graph.AddVertex(a);
            Graph.AddVertex(b);
            Graph.AddEdge(new Edge<Product>(a, b, destinationArrow:"aassa"));

            InitializeComponent();


        }
    }
}
