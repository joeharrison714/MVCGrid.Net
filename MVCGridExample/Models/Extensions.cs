using MVCGrid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCGrid.Web.Models
{
    public static class Extensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
            this IQueryable<TSource> source,
            System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector,
            SortDirection order)
        {
            switch (order)
            {
                case SortDirection.Unspecified:
                case SortDirection.Asc: return source.OrderBy(keySelector);
                case SortDirection.Dsc: return source.OrderByDescending(keySelector);
            }

            throw new ArgumentOutOfRangeException("order");
        }
    }
}