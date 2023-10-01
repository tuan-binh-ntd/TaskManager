using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TaskManager.Core.Helper;

namespace TaskManager.Core.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Add WHERE statment when condition is true
        /// </summary>
        /// <typeparam name="TSource">Type of source</typeparam>
        /// <param name="source"></param>
        /// <param name="condition">Condition</param>
        /// <param name="predicate"></param>
        /// <returns>return IQueryable</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Add WHERE statment when condition is true
        /// </summary>
        /// <typeparam name="TSource">Type of source</typeparam>
        /// <param name="source"></param>
        /// <param name="condition">Condition</param>
        /// <param name="predicate"></param>
        /// <returns>return IQueryable</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Ceiling total pages
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private static int Ceiling(int pageSize, int totalCount)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }

        /// <summary>
        /// Add paging query
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IQueryable<TSource> PageBy<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] PaginationInput input)
        {
            return source.Skip(input.PageSize * (input.PageNum - 1)).Take(input.PageSize);
        }

        /// <summary>
        /// Asynchronous paging
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PaginationResult<TSource>> Pagination<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] PaginationInput input, CancellationToken cancellationToken = default)
        {
            int totalCount = await source.CountAsync(cancellationToken: cancellationToken);

            source = source.PageBy(input);

            PaginationResult<TSource> data = new()
            {
                TotalCount = totalCount,
                TotalPage = Ceiling(input.PageSize, totalCount),
                Content = await source.ToListAsync(cancellationToken: cancellationToken)
            };

            return data;
        }

        /// <summary>
        /// Synchronous paging
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PaginationResult<TSource> Paging<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] PaginationInput input)
        {
            int totalCount = source.Count();

            source = source.PageBy(input);

            PaginationResult<TSource> data = new()
            {
                TotalCount = totalCount,
                TotalPage = Ceiling(input.PageSize, totalCount),
                Content = source.ToList()
            };

            return data;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if(condition)
            {
                return source.Where(predicate);
            }
            return source;
        }
    }
}
