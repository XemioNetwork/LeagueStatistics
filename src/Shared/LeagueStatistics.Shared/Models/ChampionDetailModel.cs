using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Models
{
    public class ChampionDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public double MovementSpeed { get; set; }
        public double AttackRange { get; set; }
        public double Mana { get; set; }
        public double ManaPerLevel { get; set; }
        public double ManaRegeneration { get; set; }
        public double ManaRegenerationPerLevel { get; set; }
        public double Health { get; set; }
        public double HealthPerLevel { get; set; }
        public double HealthRegeneration { get; set; }
        public double HealthRegenerationPerLevel { get; set; }
        public double MagicResist { get; set; }
        public double MagicResistPerLevel { get; set; }
        public double Armor { get; set; }
        public double ArmorPerLevel { get; set; }
        public double AttackDamage { get; set; }
        public double AttackDamagePerLevel { get; set; }
        public double AttackSpeedOffset { get; set; }
        public double AttackSpeedPerLevel { get; set; }
        public double Crit { get; set; }
        public double CritPerLevel { get; set; }
    }
}
