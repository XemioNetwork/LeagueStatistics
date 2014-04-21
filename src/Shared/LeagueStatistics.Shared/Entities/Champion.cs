using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Champion : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the movement speed.
        /// </summary>
        public double MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets the attack range.
        /// </summary>
        public double AttackRange { get; set; }

        /// <summary>
        /// Gets or sets the mana.
        /// </summary>
        public double Mana { get; set; }
        /// <summary>
        /// Gets or sets the mana per level.
        /// </summary>
        public double ManaPerLevel { get; set; }
        /// <summary>
        /// Gets or sets the mana regeneration.
        /// </summary>
        public double ManaRegeneration { get; set; }
        /// <summary>
        /// Gets or sets the mana regeneration per level.
        /// </summary>
        public double ManaRegenerationPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        public double Health { get; set; }
        /// <summary>
        /// Gets or sets the health per level.
        /// </summary>
        public double HealthPerLevel { get; set; }
        /// <summary>
        /// Gets or sets the health regeneration.
        /// </summary>
        public double HealthRegeneration { get; set; }
        /// <summary>
        /// Gets or sets the health regeneration per level.
        /// </summary>
        public double HealthRegenerationPerLevel { get; set; }


        /// <summary>
        /// Gets or sets the magic resist.
        /// </summary>
        public double MagicResist { get; set; }
        /// <summary>
        /// Gets or sets the magic resist per level.
        /// </summary>
        public double MagicResistPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the armor.
        /// </summary>
        public double Armor { get; set; }
        /// <summary>
        /// Gets or sets the armor per level.
        /// </summary>
        public double ArmorPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the attack damage.
        /// </summary>
        public double AttackDamage { get; set; }
        /// <summary>
        /// Gets or sets the attack damage per level.
        /// </summary>
        public double AttackDamagePerLevel { get; set; }

        /// <summary>
        /// Gets or sets the attack speed offset.
        /// </summary>
        public double AttackSpeedOffset { get; set; }
        /// <summary>
        /// Gets or sets the attack speed per level.
        /// </summary>
        public double AttackSpeedPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the crit.
        /// </summary>
        public double Crit { get; set; }
        /// <summary>
        /// Gets or sets the crit per level.
        /// </summary>
        public double CritPerLevel { get; set; }

        #region Methods
        /// <summary>
        /// Returns the display name.
        /// </summary>
        public string GetDisplayName()
        {
            return this.Name + this.Title;
        }
        #endregion
    }
}
