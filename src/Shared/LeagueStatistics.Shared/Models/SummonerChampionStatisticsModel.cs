using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Models
{
    public class SummonerChampionStatisticsModel : SummonerStatisticsModel
    {
        public string ChampionId { get; set; }
        public string ChampionName { get; set; }
        public string ChampionTitle { get; set; }
        public string ChampionImageUrl { get; set; }
    }
}
