using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.View.UserControls;
using HomeEconomicSystem.Utils;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class PieDropDownViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
        private SeriesCollection _series;

        public PieDropDownViewModel(SeriesCollection pieSeriesCollection, string titleX, string titleY)
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            Navigation = new List<SeriesCollection>();

            if(pieSeriesCollection.Count() == 1)
            {
                var pieSeries = pieSeriesCollection.First() as PieSeries;
                var dropDownPoint = pieSeries.Values.Cast<DropDownPoint>().First();
                MoveToChildSeriesCollectionOf(titleX, dropDownPoint, pieSeries);
                HasContent = false;
            }
            else
            {
                HasContent = true;
                Series = pieSeriesCollection;
            }

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
                MoveToChildSeriesCollectionOf(titleX, dropDownPoint, seriesExample);
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
        }

        private void MoveToChildSeriesCollectionOf(string titleX, DropDownPoint dropDownPoint, PieSeries seriesExample)
        {
            Series = dropDownPoint.Content
                .Select(value => new PieSeries
                {
                    Values = new ChartValues<double> { value.Value },
                    Title = titleX + " " + value.Key,
                    LabelPoint = seriesExample.LabelPoint,
                    DataLabels = seriesExample.DataLabels,
                    PushOut = seriesExample.PushOut,
                })
                .AsSeriesCollection();
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

        private void SetProperty<T>(ref T property, T value)
        {
            _notifyPropertyChanged.SetProperty(ref property, value);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}