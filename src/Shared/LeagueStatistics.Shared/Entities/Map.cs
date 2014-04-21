using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Map : AggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Map(int id, string name)
        {
            this.Id = "Maps/" + id;
            this.Name = name;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            
        }
        #endregion

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
