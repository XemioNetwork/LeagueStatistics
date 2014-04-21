using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Match : AggregateRoot
    {
        public string SummonerId { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<FellowPlayer> FellowPlayers { get; set; }

        public Team Team { get; set; }
        public GameMode GameMode { get; set; }
        public GameType GameType { get; set; }
        public GameSubType GameSubType { get; set; }

        public string MapId { get; set; }

        public string ChampionId { get; set; }

        public string FirstSpellId { get; set; }
        public string SecondSpellId { get; set; }

        public bool Win { get; set; }
        public int IPEarned { get; set; }
        public TimeSpan Length { get; set; }

        public int TotalHeal { get; set; }

        public int TotalDamageDealt { get; set; }
        public int TrueDamageDealt { get; set; }
        public int PhysicalDamageDealt { get; set; }
        public int MagicDamageDealt { get; set; }

        public int TotalDamageDealtToChampions { get; set; }
        public int TrueDamageDealtToChampions { get; set; }
        public int PhysicalDamageDealtToChampions { get; set; }
        public int MagicDamageDealtToChampions { get; set; }

        public int TotalDamageTaken { get; set; }
        public int TrueDamageTaken { get; set; }
        public int PhysicalDamageTaken { get; set; }
        public int MagicDamageTaken { get; set; }

        public int Level { get; set; }
        public int GoldEarned { get; set; }
        public int GoldSpent { get; set; }

        public string Item1Id { get; set; }
        public string Item2Id { get; set; }
        public string Item3Id { get; set; }
        public string Item4Id { get; set; }
        public string Item5Id { get; set; }
        public string Item6Id { get; set; }

        public int SightWardsBought { get; set; }
        public int WardsPlaced { get; set; }
        public int WardsKilled { get; set; }

        public int ChampionsKilled { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public int MinionsKilled { get; set; }
        public int NeutralMinionsKilledYourJungle { get; set; }
        public int NeutralMinionsKilledEnemyJungle { get; set; }
        public int LargestMultiKill { get; set; }
    }
}
