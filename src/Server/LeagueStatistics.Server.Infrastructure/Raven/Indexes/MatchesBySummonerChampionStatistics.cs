using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Indexes
{
    public class MatchesBySummonerChampionStatistics : AbstractIndexCreationTask<Match, SummonerChampionStatistics>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchesBySummonerChampionStatistics"/> class.
        /// </summary>
        public MatchesBySummonerChampionStatistics()
        {
            this.Map = matches => from match in matches
                                  select new SummonerChampionStatistics
                                  {
                                      SummonerId = match.SummonerId,

                                      ChampionId = match.ChampionId,

                                      MatchCount = 1,
                                      WinCount = match.Win ? 1 : 0,
                                      LoseCount = match.Win ? 0 : 1,

                                      MaxLenght = match.Length,
                                      AvgLenght = match.Length,
                                      MinLength = match.Length,

                                      LastMatch = match.CreationDate,
                                      FirstMatch = match.CreationDate,

                                      MaxChampionsKilled = match.ChampionsKilled,
                                      AvgChampionsKilled = match.ChampionsKilled,
                                      MinChampionsKilled = match.ChampionsKilled,

                                      MaxAssists = match.Assists,
                                      AvgAssists = match.Assists,
                                      MinAssists = match.Assists,

                                      MaxDeaths = match.Deaths,
                                      AvgDeaths = match.Deaths,
                                      MinDeaths = match.Deaths,

                                      MaxGoldEarned = match.GoldEarned,
                                      AvgGoldEarned = match.GoldEarned,
                                      MinGoldEarned = match.GoldEarned,

                                      MaxMinionsKilled = match.MinionsKilled,
                                      AvgMinionsKilled = match.MinionsKilled,
                                      MinMinionsKilled = match.MinionsKilled
                                  };

            this.Reduce = matches => from match in matches
                                     group match by new { match.SummonerId, match.ChampionId } into g
                                     let matchCount = g.Sum(f => f.MatchCount)
                                     select new SummonerChampionStatistics
                                     {
                                         SummonerId = g.Key.SummonerId,
                                         ChampionId = g.Key.ChampionId,

                                         MatchCount = matchCount,
                                         WinCount = g.Sum(f => f.WinCount),
                                         LoseCount = g.Sum(f => f.LoseCount),

                                         MaxLenght = g.Max(f => f.MaxLenght),
                                         AvgLenght = TimeSpan.FromTicks(g.Sum(f => f.AvgLenght.Ticks) / matchCount),
                                         MinLength = g.Min(f => f.MinLength),

                                         LastMatch = g.Max(f => f.LastMatch),
                                         FirstMatch = g.Min(f => f.FirstMatch),

                                         MaxChampionsKilled = g.Max(f => f.MaxChampionsKilled),
                                         AvgChampionsKilled = g.Sum(f => f.AvgChampionsKilled) / matchCount,
                                         MinChampionsKilled = g.Min(f => f.MinChampionsKilled),

                                         MaxAssists = g.Max(f => f.MaxAssists),
                                         AvgAssists = g.Sum(f => f.AvgAssists) / matchCount,
                                         MinAssists = g.Min(f => f.MinAssists),

                                         MaxDeaths = g.Max(f => f.MaxDeaths),
                                         AvgDeaths = g.Sum(f => f.AvgDeaths) / matchCount,
                                         MinDeaths = g.Min(f => f.MinDeaths),

                                         MaxGoldEarned = g.Max(f => f.MaxGoldEarned),
                                         AvgGoldEarned = g.Sum(f => f.AvgGoldEarned) / matchCount,
                                         MinGoldEarned = g.Min(f => f.MinGoldEarned),

                                         MaxMinionsKilled = g.Max(f => f.MaxMinionsKilled),
                                         AvgMinionsKilled = g.Sum(f => f.AvgMinionsKilled) / matchCount,
                                         MinMinionsKilled = g.Min(f => f.MinMinionsKilled)
                                     };
        }
        #endregion

        #region Overrides of AbstractIndexCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Matches/BySummonerChampionStatistics"; }
        }
        #endregion
    }
}
