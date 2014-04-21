namespace LeagueStatistics.Shared.Entities
{
    public class FellowPlayer
    {
        public FellowPlayer(string championId, string summonerId, Team team)
        {
            ChampionId = championId;
            SummonerId = summonerId;
            Team = team;
        }

        public FellowPlayer()
        {
            
        }

        public string ChampionId { get; set; }
        public string SummonerId { get; set; }
        public Team Team { get; set; }
    }
}