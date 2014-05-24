using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Server.Infrastructure.Controllers;
using LeagueStatistics.Shared.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Indexes
{
    public class MatchesBySummonerId : AbstractIndexCreationTask<Match>
    {
        #region Result
        /// <summary>
        /// The index result.
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Gets or sets the summoner ids.
            /// </summary>
            public List<string> SummonerIds { get; set; }
            /// <summary>
            /// Gets or sets the creation date.
            /// </summary>
            public DateTimeOffset CreationDate { get; set; }
            /// <summary>
            /// Gets or sets the game mode.
            /// </summary>
            public GameMode GameMode { get; set; }
            /// <summary>
            /// Gets or sets the type of the game.
            /// </summary>
            public GameType GameType { get; set; }
            /// <summary>
            /// Gets or sets the sub type of the game.
            /// </summary>
            public GameSubType GameSubType { get; set; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchesBySummonerId"/> class.
        /// </summary>
        public MatchesBySummonerId()
        {
            this.Map = maps => from map in maps
                               select new Result
                               {
                                   SummonerIds = map.Teams.SelectMany(f => f.Players).Select(f => f.SummonerId).ToList(),
                                   CreationDate = map.CreationDate,
                                   GameMode = map.GameMode,
                                   GameType = map.GameType,
                                   GameSubType = map.GameSubType
                               };

            this.Sort(f => f.CreationDate, SortOptions.String);
        }
        #endregion

        #region Overrides of AbstractIndexCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Matches/BySummonerIds"; }
        }
        #endregion
    }
}
