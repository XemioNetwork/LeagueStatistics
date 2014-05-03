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
    public class SummonerToSummonerModelTransformer : AbstractTransformerCreationTask<Summoner>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SummonerToSummonerModelTransformer"/> class.
        /// </summary>
        public SummonerToSummonerModelTransformer()
        {
            this.TransformResults = summoners => from summoner in summoners
                                                 select new SummonerModel
                                                 {
                                                     Id = int.Parse(summoner.Id.Split('/').Last()),
                                                     Name = summoner.Name
                                                 };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string TransformerName
        {
            get { return "Summoner/ToSummonerModel"; }
        }
        #endregion
    }
}
