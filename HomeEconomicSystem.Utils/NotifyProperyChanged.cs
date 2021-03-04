using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Utils
{
    public sealed class NotifyProperyChanged
    {
        private object _parent;
        private Action<PropertyChangedEventArgs> _onPropertyChanged;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="onPropertyChanged">Function to active when SetProperty active.</param>
        public NotifyProperyChanged(object parent, Action<PropertyChangedEventArgs> onPropertyChanged)
        {
            _parent = parent;
            _onPropertyChanged = onPropertyChanged;
        }

        /// <summary>
        /// Sets property if it does not equal existing value. Notifies listeners if change occurs.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="member">The property's backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        public bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }

            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property, used to notify listeners.</param>
        private void OnPropertyChanged(string propertyName)
            => _onPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

}
