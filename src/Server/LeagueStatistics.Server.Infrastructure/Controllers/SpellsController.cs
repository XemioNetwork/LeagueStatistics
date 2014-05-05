using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Abstractions.Services;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Server.Infrastructure.Filters;
using LeagueStatistics.Server.Infrastructure.Raven.Transformers;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Controllers
{
    public class SpellsController : BaseController
    {
        #region Fields
        private readonly ILeagueService _leagueService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="leagueService">The league service.</param>
        public SpellsController(IDocumentSession documentSession, ILeagueService leagueService) 
            : base(documentSession)
        {
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this._leagueService = leagueService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all spells.
        /// </summary>
        [Route("Spells")]
        public IHttpActionResult GetSpells()
        {
            var spells = this.DocumentSession.Query<Spell>()
                .TransformWith<SpellToSpellModelTransformer, SpellModel>()
                .OrderBy(f => f.Id)
                .ToList();

            return Ok(spells);
        }
        /// <summary>
        /// Returns the spell with the specified <paramref name="spellId"/>.
        /// </summary>
        /// <param name="spellId">The spell identifier.</param>
        [Route("Spells/{spellId:int}")]
        public IHttpActionResult GetSpell(int spellId)
        {
            Condition.Requires(spellId, "spellId")
                .IsGreaterThan(0);

            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Spell>(spellId);
            var spell = this.DocumentSession.Load<SpellToSpellModelTransformer, SpellModel>(stringId);

            if (spell == null)
                return NotFound();

            return Ok(spell);
        }
        /// <summary>
        /// Updates all spell data.
        /// </summary>
        [Route("Spells/Update")]
        [RequiresAuthentication]
        public async Task<IHttpActionResult> GetUpdateSpells()
        {
            IEnumerable<Spell> spells = await this._leagueService.GetSpellsAsync();
            foreach (var spell in spells)
            {
                this.DocumentSession.Store(spell);
            }

            return Ok();
        }
        #endregion
    }
}
