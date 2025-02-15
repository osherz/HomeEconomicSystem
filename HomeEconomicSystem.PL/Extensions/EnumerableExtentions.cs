﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.Extensions
{
    public static class EnumerableExtentions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static IReadOnlyCollection<KeyValuePair<T, string>> ToKeyValuePair<T>(this IEnumerable<T> collection)
        {
            return collection
                .Select(s => new KeyValuePair<T, string>(s, s.ToString()))
                .ToList();
        }

    }
}
