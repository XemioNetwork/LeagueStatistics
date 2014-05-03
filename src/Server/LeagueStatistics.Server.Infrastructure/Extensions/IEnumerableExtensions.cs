using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Linq;

namespace LeagueStatistics.Server.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Pages the specified queryable.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="page">The page starting with 1.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> queryable, int page, int pageSize)
        {
            return queryable
                .Skip((page - 1)*pageSize)
                .Take(pageSize);
        }
    }
}
