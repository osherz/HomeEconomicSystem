using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.View.UserControls;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class PieDropDownViewModel : NotifyPropertyChanged
    {
        private SeriesCollection _series;

        public PieDropDownViewModel(SeriesCollection pieSeriesCollection, string titleX, string titleY)
        {
            Navigation = new List<SeriesCollection>();

            Series = pieSeriesCollection;

            SliceClickCommand = new PieDropDownCommand(dropDownPoint =>
            {
                //if the point has no content to display...
                HasContent = false;

                Navigation.Add(Series.Select(x => new PieSeries
                {
                    Values = x.Values,
                    Title = x.Title,
                    Fill = ((Series)x).Fill,
                    LabelPoint = x.LabelPoint,
                    DataLabels = x.DataLabels,
                    PushOut = (x as PieSeries).PushOut
                }).AsSeriesCollection());

                var seriesExample = Navigation.First().First() as PieSeries;
                Series = dropDownPoint.Content
                    .Select(value => new PieSeries
                    {
                        Values = new ChartValues<double> { value.Value },
                        Title = titleX + " " + value.Key,
                        LabelPoint = seriesExample.LabelPoint,
                        DataLabels = seriesExample.DataLabels,
                        PushOut= seriesExample.PushOut,
                    })
                    .AsSeriesCollection();
            });

            GoBackCommand = new RelayCommand(obj =>
            {
                if (!Navigation.Any()) return;
                var previous = Navigation.Last();
                if (previous == null) return;
                Series = previous;
                Navigation.Remove(previous);
                HasContent = true;
            }, () =>
            {
                return Navigation.Count >= 1;
            });

            Formatter = x => titleY + " " + x.ToString();
            HasContent = true;
        }

        public List<SeriesCollection> Navigation { get; }

        private bool _hasContent;
        public bool HasContent
        {
            get { return _hasContent; }
            set { SetProperty(ref _hasContent, value); }
        }

        public SeriesCollection Series
        {
            get { return _series; }
            set => SetProperty(ref _series, value);
        }
        public PieDropDownCommand SliceClickCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}