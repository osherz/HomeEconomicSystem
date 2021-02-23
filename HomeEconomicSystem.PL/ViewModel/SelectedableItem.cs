using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class SelectedableItem<T> : NotifyPropertyChanged
    {
        public T Item { get; }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public SelectedableItem(T item)
        {
            Item = item;
            IsSelected = false;
        }
    }
}
