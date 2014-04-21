using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ChampionsController : BaseController
    {
        #region Fields
        private readonly ILeagueService _leagueService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChampionsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="leagueService">The league service.</param>
        public ChampionsController(IDocumentSession documentSession, ILeagueService leagueService) 
            : base(documentSession)
        {
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this._leagueService = leagueService;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Updates all champion data.
        /// </summary>
        [Route("Champions/Update")]
        [RequiresAuthentication]
        public async Task<IHttpActionResult> GetUpdateChampions()
        {
            IEnumerable<Champion> champions = await this._leagueService.GetChampionsAsync();
            foreach (var champion in champions)
            {
                this.DocumentSession.Store(champion);
            }

            return Ok();
        }
        #endregion
    }
}
