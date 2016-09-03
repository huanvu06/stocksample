using CrossOverStock.Data.Paging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Attach(T entity, EntityState state = EntityState.Unchanged);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> items);
        void DeleteRange(Expression<Func<T, bool>> criteria);
        void SoftDelete(T entity);

        T FindOne(Expression<Func<T, bool>> criteria);
        T FindOne();
        T FindOne(long id);
        IQueryable<T> Get(Expression<Func<T, bool>> criteria, bool includeHidden = false);
        IQueryable<T> Get(bool includeHidden = false);
                
        IQueryable<T> GetPaging(List<SortExpression<T>> sortExpressions, out int totalRecord,
            Expression<Func<T, bool>> predicate = null, string[] includePaths = null,
            int? page = 0, int? pageSize = null, bool includeHidden = false);
        
        bool HasChange();
        Action<T> OnChange { get; set; }
        int ExecWithStoreProcedure(string query, params object[] parameters);        
        void BulkInsert(IEnumerable<T> items, bool keepIdentity = false, string identityFieldName = "ID");
        void BulkInsert(System.Data.DataTable table);
        void BulkUpdateOneField(IEnumerable<T> entities, string fieldName, string fieldValue);        
    }
}
