using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Models
{
    public class SummonerStatistics
    {
        public string SummonerId { get; set; }

        public int MatchCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }

        public TimeSpan MaxLenght { get; set; }
        public TimeSpan AvgLenght { get; set; }
        public TimeSpan MinLength { get; set; }

        public DateTimeOffset LastMatch { get; set; }
        public DateTimeOffset FirstMatch { get; set; }

        public double MaxChampionsKilled { get; set; }
        public double AvgChampionsKilled { get; set; }
        public double MinChampionsKilled { get; set; }

        public double MaxAssists { get; set; }
        public double AvgAssists { get; set; }
        public double MinAssists { get; set; }

        public double MaxDeaths { get; set; }
        public double AvgDeaths { get; set; }
        public double MinDeaths { get; set; }

        public double MaxGoldEarned { get; set; }
        public double AvgGoldEarned { get; set; }
        public double MinGoldEarned { get; set; }

        public double MaxMinionsKilled { get; set; }
        public double AvgMinionsKilled { get; set; }
        public double MinMinionsKilled { get; set; }
    }
}
