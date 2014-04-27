using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Connection;

namespace LeagueStatistics.Server.Infrastructure.Extensions
{
    public static class IDatabaseCommandsExtensions
    {
        /// <summary>
        /// Returns whether the document exists.
        /// </summary>
        /// <param name="databaseCommands">The database commands.</param>
        /// <param name="documentId">The document identifier.</param>
        public static bool DocumentExists(this IDatabaseCommands databaseCommands, string documentId)
        {
            return databaseCommands.Head(documentId) != null;
        }
    }
}
