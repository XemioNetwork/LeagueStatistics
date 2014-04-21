using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Abstractions.Services;
using LeagueStatistics.Server.Infrastructure.Filters;
using LeagueStatistics.Shared.Entities;
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
