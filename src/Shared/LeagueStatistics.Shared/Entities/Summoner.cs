using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Summoner : AggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Summoner"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Summoner(long id, string name)
        {
            this.Id = "Summoners/" + id;
            this.Name = name;

            this.NextMatchImportDate = DateTimeOffset.UtcNow;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Summoner"/> class.
        /// </summary>
        public Summoner()
        {
            
        }
        #endregion

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the last import date.
        /// </summary>
        public DateTimeOffset NextMatchImportDate { get; set; }
    }
}
