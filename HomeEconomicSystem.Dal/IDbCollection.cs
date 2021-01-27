
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeEconomicSystem.Dal
{
    public interface IDbCollection<T> : IQueryable<T>, IEnumerable<T>
        where T: class
    {
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
    }
}