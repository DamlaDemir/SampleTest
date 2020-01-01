using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.infrastructure.Repository
{
    public interface IRepository<T> where T :class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> list);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> list);
        void ChangeState(T entity, EntityState state);
        void Delete(T entity);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> list);
        void DeleteRange(IEnumerable<int> list);
        //int ExecQuery(string query, params object[] parameters);
        //Task<int> ExecQueryAsync(string query, params object[] parameters);
        //DataTable GetDataTable(string query);
        //Task<DataTable> GetDataTableAsync(string query);
        T GetSingle(Expression<Func<T, bool>> expression);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);
        //IList<T> GetSqlQueryResult(string query, params object[] parameters);
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        IQueryable<T> Query(Expression<Func<T, bool>> expression = null, bool showDeletedRows = false);
        void QuickUpdate(T original, T updated);
        //IList<T> SqlQuery(string query);
        //Task<IList<T>> SqlQueryAsync(string query);
        void Update(T entity);
    }
}
