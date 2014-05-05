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
    public class SpellToSpellModelTransformer : AbstractTransformerCreationTask<Spell>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellToSpellModelTransformer"/> class.
        /// </summary>
        public SpellToSpellModelTransformer()
        {
            this.TransformResults = spells => from spell in spells
                                              select new SpellModel
                                              {
                                                  Id = int.Parse(spell.Id.Split('/').Last()),
                                                  Name = spell.Name,
                                                  Description = spell.Description,
                                                  Cooldown = spell.Cooldown,
                                                  ImageUrl = spell.ImageUrl,
                                                  SummonerLevel = spell.SummonerLevel
                                              };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the transformer.
        /// </summary>
        public override string TransformerName
        {
            get { return "Spell/ToSpellModel"; }
        }
        #endregion
    }
}
