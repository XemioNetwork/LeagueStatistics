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
    public class ChampionToChampionModelTransformer : AbstractTransformerCreationTask<Champion>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChampionToChampionModelTransformer"/> class.
        /// </summary>
        public ChampionToChampionModelTransformer()
        {
            this.TransformResults = champions => from champion in champions
                                                 select new ChampionModel
                                                 {
                                                     Id = int.Parse(champion.Id.Split('/').Last()),
                                                     ImageUrl = champion.ImageUrl,
                                                     Name = champion.Name,
                                                     Title = champion.Title
                                                 };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string TransformerName
        {
            get { return "Champion/ToChampionModel"; }
        }
        #endregion
    }
}
