using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Transformers
{
    public class MapToMapModelTransformer : AbstractTransformerCreationTask<Map>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MapToMapModelTransformer"/> class.
        /// </summary>
        public MapToMapModelTransformer()
        {
            this.TransformResults = maps => from map in maps
                                            select new MapModel
                                            {
                                                Id = int.Parse(map.Id.Split('/').Last()),
                                                Name = map.Name
                                            };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the transformer.
        /// </summary>
        public override string TransformerName
        {
            get { return "Map/ToMapModel"; }
        }
        #endregion
    }
}
