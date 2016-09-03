using CrossOverStock.Data.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NLog;
using CrossOverStock.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossOverStock.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T:BaseEntity
    {
        protected readonly string TableName;
        protected static ILogger logger = LogManager.GetLogger("BaseRepository");
        protected DbContext Context;
        protected readonly DbSet<T> DbSet;
        protected ICurrentUser CurrentUser;
        
        public Action<T> OnChange { get; set; }
        

        public BaseRepository(DbContext context, ICurrentUser currentUser)
        {
            Context = context;
            TableName = (Context as IObjectContextAdapter).ObjectContext.CreateObjectSet<T>().EntitySet.Name;
            CurrentUser = currentUser;
            DbSet = Context.Set<T>();
        }

        protected bool HasIsDeleted
        {
            get
            {
                if (_hasIsDeleted == null)
                {
                    _hasIsDeleted = false;
                    var pIsDeleted = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "IsDeleted");
                    if (pIsDeleted != null && !Attribute.IsDefined(pIsDeleted, typeof(NotMappedAttribute)))
                        _hasIsDeleted = true;
                }

                return _hasIsDeleted.Value;
            }
        }
        bool? _hasIsDeleted;

        private void RaiseOnChange(T entity)
        {
            OnChange?.Invoke(entity);
        }

        public virtual void Insert(T entity)
        {
            entity.CreatedOnDate = DateTime.UtcNow;
            entity.CreatedByUserId = CurrentUser.UserId;
            DbSet.Add(entity);
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
            else
                SetDefaultValues(entity);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
            else
                SetDefaultValues(entity);
        }

        public virtual void Attach(T entity, EntityState state = EntityState.Unchanged)
        {
            DbSet.Attach(entity);
            switch (state)
            {
                case EntityState.Added:
                    Context.Entry(entity).State = EntityState.Added;
                    break;
                case EntityState.Deleted:
                    Context.Entry(entity).State = EntityState.Deleted;
                    break;
                case EntityState.Modified:
                    Context.Entry(entity).State = EntityState.Modified;
                    break;
                default:
                    Context.Entry(entity).State = EntityState.Unchanged;
                    break;
            }
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
        }
        public virtual void DeleteRange(IEnumerable<T> items)
        {
            DbSet.RemoveRange(items);
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
        }
        public virtual void DeleteRange(Expression<Func<T, bool>> criteria)
        {
            var items = DbSet.Where(criteria);
            DbSet.RemoveRange(items);
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
        }
        public virtual T FindOne(Expression<Func<T, bool>> criteria)
        {
            return DbSet.Where(criteria).FirstOrDefault();
        }

        public virtual T FindOne()
        {
            return DbSet.FirstOrDefault();
        }
        public virtual T FindOne(long id)
        {
            return FindOne(x => x.Id == id);
        }
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> criteria, bool includeHidden = false)
        {
            return Get(includeHidden).Where(criteria);
        }

        public virtual IQueryable<T> Get(bool includeHidden = false)
        {
            return !includeHidden && HasIsDeleted ? DbSet.Where(x => x.IsDeleted == false) : DbSet;
        }
        
        public IQueryable<T> GetPaging(List<SortExpression<T>> sortExpressions, out int totalRecord,
          Expression<Func<T, bool>> predicate = null, string[] includePaths = null,
          int? pageNumber = 1, int? pageSize = 10, bool includeHidden = false)
        {
            IQueryable<T> query = Get(includeHidden);

            if (predicate != null)
            {
                query = query.Where(predicate);

            }
            totalRecord = query.Count();


            if (includePaths != null)
            {
                for (var i = 0; i < includePaths.Count(); i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }

            if (sortExpressions != null)
            {
                IOrderedQueryable<T> orderedQuery = null;

                for (var i = 0; i < sortExpressions.Count(); i++)
                {
                    if (i == 0)
                    {
                        orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? query.OrderBy(sortExpressions[i].SortBy) : query.OrderByDescending(sortExpressions[i].SortBy);
                    }
                    else
                    {
                        if (orderedQuery != null)
                            orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? orderedQuery.ThenBy(sortExpressions[i].SortBy) : orderedQuery.ThenByDescending(sortExpressions[i].SortBy);
                    }
                }



                if (pageNumber != null)
                {
                    if (pageSize != null)
                        query = orderedQuery.Skip(((int)pageNumber - 1) * (int)pageSize);
                }
            }

            if (pageSize != null)
            {
                query = query.Take((int)pageSize);
            }
            return query;
        }

        public void SaveOrUpdate(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Add(entity);
            }
            else
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            Context.SaveChanges();
        }

        public virtual void SaveChanges()
        {
            try
            {           
                foreach (
                    var entry in
                        Context.ChangeTracker.Entries<T>()
                            .Where(
                                item =>
                                    item.State == EntityState.Added || item.State == EntityState.Modified ||
                                    item.State == EntityState.Deleted))
                {
                    var entity = entry.Entity;
                    SetDefaultValues(entity);
                    RaiseOnChange(entity);
                }
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    logger.Error(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    var string1 = eve.Entry.Entity.GetType().Name;
                    var string2 = eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        logger.Error(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                        var string3 = ve.PropertyName;
                        var string4 = ve.ErrorMessage;
                    }
                }
                throw;
            }
        }

        void SetDefaultValues(T entity)
        {
            entity.LastModifiedOnDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = CurrentUser.UserId;
        }

        public void SoftDelete(T entity)
        {
            var entry = Context.Entry(entity);
            entry.State = EntityState.Detached;
            entity.IsDeleted = true;
            if (Context.Database.CurrentTransaction == null)
                SaveChanges();
            else
                SetDefaultValues(entity);
        }

        public bool HasChange()
        {
            return
                Context.ChangeTracker.Entries<T>()
                    .Where(
                        item =>
                            item.State == EntityState.Added || item.State == EntityState.Modified ||
                            item.State == EntityState.Deleted)
                    .Select(entry => entry.Entity)
                    .Any();
        }
        public int ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return Context.Database
               .ExecuteSqlCommand(query, parameters); ;

            //return DbSet.SqlQuery(query, parameters).ToList();
        }
        
        public void BulkInsert(IEnumerable<T> items, bool keepIdentity = false, string identityFieldName = "ID")
        {
            //connection string is the only way this works, don't try reusing the connection from Entity Framework
            try
            {
                using (var bulkCopy = new SqlBulkCopy(Context.Database.Connection.ConnectionString))
                {
                    bulkCopy.BatchSize = items.Count();
                    bulkCopy.DestinationTableName = TableName;

                    var table = new System.Data.DataTable();
                    var props = TypeDescriptor.GetProperties(typeof(T))
                                               //Dirty hack to make sure we only have system data types 
                                               //i.e. filter out the relationships/collections
                                               .Cast<PropertyDescriptor>()
                                               .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                               .ToArray();

                    if (keepIdentity)
                        props = props.Where(x => x.Name != identityFieldName).ToArray();

                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                    }

                    var values = new object[props.Length];
                    foreach (var item in items)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }

                        table.Rows.Add(values);
                    }

                    try
                    {
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        //fallback to insert one-by-one (ambient transactions will cause SqlBulkCopy to fail)
                        foreach (T item in items)
                        {
                            DbSet.Add(item);
                        }
                        Context.SaveChanges();
                        logger.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void BulkInsert(System.Data.DataTable table)
        {
            //connection string is the only way this works, don't try reusing the connection from Entity Framework
            try
            {
                using (var bulkCopy = new SqlBulkCopy(Context.Database.Connection.ConnectionString))
                {
                    bulkCopy.BatchSize = table.Rows.Count;
                    bulkCopy.DestinationTableName = table.TableName;
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(table);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public virtual void BulkUpdateOneField(IEnumerable<T> entities, string fieldName, string fieldValue)
        {
            StringBuilder ids = new StringBuilder();
            foreach (T item in entities)
            {
                if (ids.Length > 0)
                    ids.Append(", ");

                ids.Append(item.Id);
            }

            string query = string.Format("UPDATE {0} SET {1} = {2} WHERE ID in ({3})", TableName, fieldName, fieldValue, ids);
            Context.Database.ExecuteSqlCommand(query);
        }
    }    
}
