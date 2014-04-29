﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Abstractions.Indexing;
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
                                  let summoner = LoadDocument<Summoner>(match.SummonerId)
                                  let champion = LoadDocument<Champion>(match.ChampionId)
                                  select new SummonerChampionStatistics
                                  {
                                      SummonerId = match.SummonerId,
                                      SummonerName = summoner.Name,

                                      ChampionId = match.ChampionId,
                                      ChampionName = champion.Name,
                                      ChampionTitle = champion.Title,
                                      ChampionImageUrl = champion.ImageUrl,

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

                                      MaxGoldPerMinute = (double)match.GoldEarned / match.Length.TotalMinutes,
                                      AvgGoldPerMinute = (double)match.GoldEarned / match.Length.TotalMinutes,
                                      MinGoldPerMinute = (double)match.GoldEarned / match.Length.TotalMinutes,

                                      MaxMinionsKilled = match.MinionsKilled,
                                      AvgMinionsKilled = match.MinionsKilled,
                                      MinMinionsKilled = match.MinionsKilled,

                                      MaxMinionsKilledPerMinute = (double)match.MinionsKilled / match.Length.TotalMinutes,
                                      AvgMinionsKilledPerMinute = (double)match.MinionsKilled / match.Length.TotalMinutes,
                                      MinMinionsKilledPerMinute = (double)match.MinionsKilled / match.Length.TotalMinutes,
                                  };

            this.Reduce = matches => from match in matches
                                     group match by new { match.SummonerId, match.ChampionId } into g
                                     let matchCount = g.Sum(f => f.MatchCount)
                                     select new SummonerChampionStatistics
                                     {
                                         SummonerId = g.Key.SummonerId,
                                         SummonerName = g.Select(f => f.SummonerName).First(),

                                         ChampionId = g.Key.ChampionId,
                                         ChampionName = g.Select(f => f.ChampionName).First(),
                                         ChampionTitle = g.Select(f => f.ChampionTitle).First(),
                                         ChampionImageUrl = g.Select(f => f.ChampionImageUrl).First(),

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

                                         MaxGoldPerMinute = g.Max(f => f.MaxGoldPerMinute),
                                         AvgGoldPerMinute = g.Sum(f => f.AvgGoldPerMinute) / matchCount,
                                         MinGoldPerMinute = g.Min(f => f.MinGoldPerMinute),

                                         MaxMinionsKilled = g.Max(f => f.MaxMinionsKilled),
                                         AvgMinionsKilled = g.Sum(f => f.AvgMinionsKilled) / matchCount,
                                         MinMinionsKilled = g.Min(f => f.MinMinionsKilled),

                                         MaxMinionsKilledPerMinute = g.Max(f => f.MaxMinionsKilledPerMinute),
                                         AvgMinionsKilledPerMinute = g.Sum(f => f.AvgMinionsKilledPerMinute) / matchCount,
                                         MinMinionsKilledPerMinute = g.Min(f => f.MinMinionsKilledPerMinute)
                                     };

            this.Index(f => f.SummonerName, FieldIndexing.Analyzed);

            this.Index(f => f.ChampionName, FieldIndexing.Analyzed);
            this.Index(f => f.ChampionTitle, FieldIndexing.Analyzed);

            this.Sort(f => f.MatchCount, SortOptions.Int);
            this.Sort(f => f.WinCount, SortOptions.Int);
            this.Sort(f => f.LoseCount, SortOptions.Int);

            this.Sort(f => f.MaxChampionsKilled, SortOptions.Double);
            this.Sort(f => f.AvgChampionsKilled, SortOptions.Double);
            this.Sort(f => f.MinChampionsKilled, SortOptions.Double);

            this.Sort(f => f.MaxAssists, SortOptions.Double);
            this.Sort(f => f.AvgAssists, SortOptions.Double);
            this.Sort(f => f.MinAssists, SortOptions.Double);

            this.Sort(f => f.MaxDeaths, SortOptions.Double);
            this.Sort(f => f.AvgDeaths, SortOptions.Double);
            this.Sort(f => f.MinDeaths, SortOptions.Double);

            this.Sort(f => f.MaxGoldPerMinute, SortOptions.Double);
            this.Sort(f => f.AvgGoldPerMinute, SortOptions.Double);
            this.Sort(f => f.MinGoldPerMinute, SortOptions.Double);

            this.Sort(f => f.MaxMinionsKilled, SortOptions.Double);
            this.Sort(f => f.AvgMinionsKilled, SortOptions.Double);
            this.Sort(f => f.MinMinionsKilled, SortOptions.Double);

            this.Sort(f => f.MaxMinionsKilledPerMinute, SortOptions.Double);
            this.Sort(f => f.AvgMinionsKilledPerMinute, SortOptions.Double);
            this.Sort(f => f.MinMinionsKilledPerMinute, SortOptions.Double);
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
