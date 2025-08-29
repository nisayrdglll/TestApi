using System.Linq.Expressions;

namespace Helpers.Helpers
{
    public static class QueryHelper
    {
        public static IOrderedQueryable<T> OrderByString<T>(this IQueryable<T> query, string memberName, bool asc = true)
        {
            ParameterExpression[] typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "") };
            System.Reflection.PropertyInfo pi = typeof(T).GetProperty(memberName);

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                Expression.Call(
                typeof(Queryable),
                asc ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), pi.PropertyType },
                query.Expression,
                Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams))
            );
        }
    }
}
