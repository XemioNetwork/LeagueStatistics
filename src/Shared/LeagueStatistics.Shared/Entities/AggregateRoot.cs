using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    /// <summary>
    /// The base class for all aggregate roots.
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
