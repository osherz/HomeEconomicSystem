using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.View.UserControls;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HomeEconomicSystem.PL.Converters
{
    public class BasicGraphToGraphDataFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IGraphManagement graphManagement = new BL.BL().GraphManagement;
            BasicGraph graph = value as BasicGraph;
            
            IReadOnlyDictionary<int, IEnumerable<(int, int)>> dataAnalyzed;
            IEnumerable<IName> names;

            if (graph is CategoryGraph)
            {
                dataAnalyzed = graphManagement.AnalyzeGraph(graph as CategoryGraph);
                names = (graph as CategoryGraph).Categories;
            }
            else if(graph is ProductGraph)
            {
                dataAnalyzed = graphManagement.AnalyzeGraph(graph as ProductGraph);
                names = (graph as ProductGraph).Products;
            }
            else if (graph is StoreGraph)
            {
                dataAnalyzed = graphManagement.AnalyzeGraph(graph as StoreGraph);
                names = (graph as StoreGraph).Stores;
            }
            else if (graph is TransactionsGraph)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }

            var allXs = GetXs(dataAnalyzed);
            return new GraphData
            {
                Title = graph.Title,
                Description = graph.Description,
                SeriesCollection = CreateSeriesCollection(dataAnalyzed, names, allXs),
                Labels = allXs.Select(num => num.ToString()).ToArray(),
                YFormatter = y => y.ToString()
            };
        }

        /// <summary>
        /// Return all Xs, sorted.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerable<T> GetXs<T, G, TKey>(IReadOnlyDictionary<TKey, IEnumerable<(T, G)>> data)
            where T: IComparable
        {
            return data.Values
                .SelectMany(points => points)
                .Select(p => p.Item1)
                .OrderBy(a=>a);
        }

        private SeriesCollection CreateSeriesCollection(IReadOnlyDictionary<int, IEnumerable<(int, int)>> data, IEnumerable<IName> seriesNames, IEnumerable<int> xs)
        {
            int maxXs = xs.Last();
            SeriesCollection seriesCollection = new SeriesCollection();
            foreach(var name in seriesNames)
            {
                seriesCollection.Add(CreateLineSeries(name, data[name.Id], maxXs));
            }
            return seriesCollection;
        }

        private LineSeries CreateLineSeries(IName item, IEnumerable<(int, int)> points, int maxXs)
        {
            var xsItems = new int[maxXs];
            foreach (var point in points)
            {
                xsItems[point.Item1] = point.Item2;
            }
            return new LineSeries
            {
                Title = item.Name,
                Values = new ChartValues<int>(xsItems)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
