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
    public class BasicGraphToGraphDataConverter : IValueConverter
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
            else if(graph is null)
            {
                return null;
            }
            else
            {
                throw new NotSupportedException();
            }

            var allXs = GetXs(dataAnalyzed);
            var graphData = new GraphData
            {
                BasicGraph = graph,
                SeriesCollection = CreateSeriesCollection(dataAnalyzed, names, allXs, graph.GraphType),
                Labels = allXs.Select(num => num.ToString()).ToArray(),
                YFormatter = y => y.ToString(),
            };
            graphData.ItemsDataForTable = from series in graphData.SeriesCollection
                                          let title = series.Title
                                          let values = series.Values.Cast<int>().Select(num=>graphData.YFormatter(num))
                                          let points = graphData.Labels.Zip(values, (x, y) => new { X = x, Y = y }).ToList()
                                          select new KeyValuePair<string, IEnumerable>(title, points);
            return graphData;
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
                .Distinct()
                .OrderBy(a=>a);
        }

        private SeriesCollection CreateSeriesCollection(IReadOnlyDictionary<int, IEnumerable<(int, int)>> data, IEnumerable<IName> seriesNames, IEnumerable<int> xs, GraphType graphType)
        {
            int maxXs = xs.Last();
            SeriesCollection seriesCollection = new SeriesCollection();
            foreach(var name in seriesNames)
            {
                seriesCollection.Add(CreateSeries(name, data[name.Id], maxXs, graphType));
            }
            return seriesCollection;
        }

        private Series CreateSeries(IName item, IEnumerable<(int, int)> points, int maxXs, GraphType graphType)
        {
            var xsItems = new int[maxXs+1];
            foreach (var point in points)
            {
                xsItems[point.Item1] = point.Item2;
            }

            Series series = CreateSeries(graphType);
            series.Title = item.Name;
            series.Values = new ChartValues<int>(xsItems);
            return series;
        }

        private Series CreateSeries(GraphType graphType)
        {
            switch (graphType)
            {
                case GraphType.Bar:
                    return new ColumnSeries();
                case GraphType.Pie:
                    return new PieSeries();
                case GraphType.Linear:
                default:
                    return new LineSeries();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
