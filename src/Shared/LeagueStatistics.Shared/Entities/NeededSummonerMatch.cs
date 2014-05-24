using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class NeededSummonerMatch : AggregateRoot
    {
        public string SummonerId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string MatchId { get; set; }
    }
}
