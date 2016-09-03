using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CrossOverStock.Data.Paging
{
    public class SortExpression<TEntity> 
    {
        public SortExpression(Expression<Func<TEntity, object>> sortBy, ListSortDirection sortDirection)
        {
            SortBy = sortBy;
            SortDirection = sortDirection;
        }

        public Expression<Func<TEntity, object>> SortBy { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }
}
