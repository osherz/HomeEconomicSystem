using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal.EntityFramework
{
    public class DbSetCollection<T> : IDbCollection<T>
        where T : class
    {
        private DbSet<T> _dbSet;

        public DbSetCollection(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public Expression Expression => (_dbSet as IQueryable).Expression;

        public Type ElementType => (_dbSet as IQueryable).ElementType;

        public IQueryProvider Provider => (_dbSet as IQueryable).Provider;

        public IEnumerator<T> GetEnumerator()
        {
            return (_dbSet as IEnumerable<T>).GetEnumerator();
        }

        void IDbCollection<T>.Add(T item)
        {
            _dbSet.Add(item);
        }

        void IDbCollection<T>.AddRange(IEnumerable<T> items)
        {
            _dbSet.AddRange(items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_dbSet as IEnumerable).GetEnumerator();
        }

        void IDbCollection<T>.Remove(T item)
        {
            _dbSet.Remove(item);
        }

        void IDbCollection<T>.RemoveRange(IEnumerable<T> items)
        {
            _dbSet.RemoveRange(items);
        }
    }
}
