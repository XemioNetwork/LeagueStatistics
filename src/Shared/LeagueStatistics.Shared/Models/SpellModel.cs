using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Models
{
    public class SpellModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cooldown { get; set; }
        public string ImageUrl { get; set; }
        public int SummonerLevel { get; set; }
    }
}
