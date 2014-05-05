using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Transformers
{
    public class ChampionToChampionDetailModelTransformer : AbstractTransformerCreationTask<Champion>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChampionToChampionDetailModelTransformer"/> class.
        /// </summary>
        public ChampionToChampionDetailModelTransformer()
        {
            this.TransformResults = champions => from champion in champions
                                                 select new ChampionDetailModel
                                                 {
                                                     Id = int.Parse(champion.Id.Split('/').Last()),
                                                     Name = champion.Name,
                                                     Title = champion.Title,
                                                     ImageUrl = champion.ImageUrl,
                                                     MovementSpeed = champion.MovementSpeed,
                                                     AttackRange = champion.AttackRange,
                                                     Mana = champion.Mana,
                                                     ManaPerLevel = champion.ManaPerLevel,
                                                     ManaRegeneration = champion.ManaRegeneration,
                                                     ManaRegenerationPerLevel = champion.ManaRegenerationPerLevel,
                                                     Health = champion.Health,
                                                     HealthPerLevel = champion.HealthPerLevel,
                                                     HealthRegeneration = champion.HealthRegeneration,
                                                     HealthRegenerationPerLevel = champion.HealthRegenerationPerLevel,
                                                     MagicResist = champion.MagicResist,
                                                     MagicResistPerLevel = champion.MagicResistPerLevel,
                                                     Armor = champion.Armor,
                                                     ArmorPerLevel = champion.ArmorPerLevel,
                                                     AttackDamage = champion.AttackDamage,
                                                     AttackDamagePerLevel = champion.AttackDamagePerLevel,
                                                     AttackSpeedOffset = champion.AttackSpeedOffset,
                                                     AttackSpeedPerLevel = champion.AttackSpeedPerLevel,
                                                     Crit = champion.Crit,
                                                     CritPerLevel = champion.CritPerLevel
                                                 };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the transformer.
        /// </summary>
        public override string TransformerName
        {
            get { return "Champion/ToChampionDetail"; }
        }
        #endregion
    }
}
