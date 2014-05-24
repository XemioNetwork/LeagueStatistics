using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Match : AggregateRoot
    {
        public DateTimeOffset CreationDate { get; set; }
        public TimeSpan Length { get; set; }

        public string MapId { get; set; }

        public GameMode GameMode { get; set; }
        public GameType GameType { get; set; }
        public GameSubType GameSubType { get; set; }

        public ICollection<Team> Teams { get; set; }
    }

    public class Team
    {
        public bool Win { get; set; }
        public TeamColor Color { get; set; }
        public ICollection<PlayerStats> Players { get; set; }
    }
}
