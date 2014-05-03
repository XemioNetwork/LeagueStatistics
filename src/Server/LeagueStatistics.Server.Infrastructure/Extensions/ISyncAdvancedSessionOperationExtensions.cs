using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Extensions
{
    public static class ISyncAdvancedSessionOperationExtensions
    {
        /// <summary>
        /// Gets the string id for the given type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="id">The id.</param>
        public static string GetStringIdFor<T>(this ISyncAdvancedSessionOperation advanced, int id)
        {
            return advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        }
    }
}
