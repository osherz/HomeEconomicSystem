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
        const double PUSH_OUT = 4;
        const string TRANSACTION_NAME = "עסקאות";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IGraphManagement graphManagement = new BL.BL().GraphManagement;
            BasicGraph graph = value as BasicGraph;

            IReadOnlyDictionary<int, IEnumerable<(double, double)>> dataAnalyzed;
            IEnumerable<IName> names;

            if (graph is CategoryGraph)
            {
                dataAnalyzed = graphManagement.AnalyzeGraph(graph as CategoryGraph);
                names = (graph as CategoryGraph).Categories;
            }
            else if (graph is ProductGraph)
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
                dataAnalyzed = graphManagement.AnalyzeGraph(graph as TransactionsGraph);
                names = new[] { new TransactionName(dataAnalyzed.Keys.First(), TRANSACTION_NAME) };
            }
            else if (graph is null)
            {
                return null;
            }
            else
            {
                throw new NotSupportedException();
            }

            var allXsSorted = GetXs(dataAnalyzed);
            var graphData = new GraphData
            {
                BasicGraph = graph,
                ColumnSeriesCollection = CreateSeriesCollection(dataAnalyzed, names, allXsSorted, values => new ColumnSeries { Values = new ChartValues<double>(values) }),
                LineSeriesCollection = CreateSeriesCollection(dataAnalyzed, names, allXsSorted, values => new LineSeries { Values = new ChartValues<double>(values) }),
                PieSeriesCollection = CreatePieSeriesCollection(dataAnalyzed, names, allXsSorted),
                Labels = allXsSorted.Select(num => num.ToString()).ToArray(),
                YFormatter = y => y.ToString(),
            };
            graphData.ItemsDataForTable = from series in graphData.ColumnSeriesCollection
                                          let title = series.Title
                                          let values = series.Values.Cast<double>().Select(num => graphData.YFormatter(num))
                                          let points = graphData.Labels.Zip(values, (x, y) => new { X = x, Y = y }).ToList()
                                          select new KeyValuePair<string, IEnumerable>(title, points);
            return graphData;
        }

        private SeriesCollection CreatePieSeriesCollection(IReadOnlyDictionary<int, IEnumerable<(double, double)>> dataAnalyzed, IEnumerable<IName> names, IEnumerable<double> allXsSorted)
        {
            return CreateSeriesCollection(dataAnalyzed, names, allXsSorted, values =>
            {
                var content = allXsSorted
                    .Select((value, index) => (value, values[index]))
                    .ToDictionary(item => item.Item1.ToString(), item => item.value);
                return new PieSeries
                {
                    Values = new ChartValues<DropDownPoint>
                    {
                        new DropDownPoint
                        {
                            Content = content
                        }
                    },
                    DataLabels=true,
                    LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation),
                    PushOut = PUSH_OUT
                };
            });
        }

        /// <summary>
        /// Return all Xs, sorted.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerable<T> GetXs<T, G, TKey>(IReadOnlyDictionary<TKey, IEnumerable<(T, G)>> data)
            where T : IComparable
        {
            var points = data.Values.First();
            return points.Select(p => p.Item1);
        }

        /// <summary>
        /// Create series collection from the given data.
        /// </summary>
        /// <typeparam name="TSeries">Pie, Bar or Line</typeparam>
        /// <param name="data"></param>
        /// <param name="seriesNames"></param>
        /// <param name="allXsSorted"></param>
        /// <param name="graphType"></param>
        /// <returns></returns>
        private SeriesCollection CreateSeriesCollection(IReadOnlyDictionary<int, IEnumerable<(double, double)>> data, IEnumerable<IName> seriesNames, IEnumerable<double> allXsSorted, Func<double[], Series> createSeries)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            foreach (var name in seriesNames)
            {
                seriesCollection.Add(CreateSeries(name, data[name.Id], allXsSorted, createSeries));
            }
            return seriesCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSeries"></typeparam>
        /// <param name="item"></param>
        /// <param name="points"></param>
        /// <param name="allXsSorted"></param>
        /// <param name="createSeries">Function that get values, insert it to a series and return the series</param>
        /// <returns></returns>
        private Series CreateSeries(IName item, IEnumerable<(double, double)> points, IEnumerable<double> allXsSorted, Func<double[], Series> createSeries)
        {
            var xIndexDict = allXsSorted
                .Select((value, index) => new KeyValuePair<double, int>(value, index))
                .ToDictionary(item => item.Key, item => item.Value);

            var yS = new double[allXsSorted.Count()];
            foreach (var point in points)
            {
                yS[xIndexDict[point.Item1]] = point.Item2;
            }

            Series series = createSeries(yS);
            series.Title = item.Name;
            return series;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        class TransactionName : IName
        {
            public int Id {get;}
            public string Name { get; }

            public TransactionName(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }
}
