using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        /// Returns all champions.
        /// </summary>
        [Route("Champions")]
        public IHttpActionResult GetChampions()
        {
            var champions = this.DocumentSession.Query<Champion>()
                .TransformWith<ChampionToChampionModelTransformer, ChampionModel>()
                .OrderBy(f => f.Id)
                .ToList();

            return Ok(champions);
        }
        /// <summary>
        /// Returns the champion with the specified <paramref name="championId"/>.
        /// </summary>
        /// <param name="championId">The champion identifier.</param>
        [Route("Champions/{championId:int}")]
        public IHttpActionResult GetChampion(int championId)
        {
            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Champion>(championId);
            var details = this.DocumentSession.Load<ChampionToChampionModelTransformer, ChampionModel>(stringId);

            if (details == null)
                return NotFound();

            return Ok(details);
        }
        /// <summary>
        /// Returns the details for the champion with the specified <paramref name="championId"/>.
        /// </summary>
        /// <param name="championId">The champion identifier.</param>
        [Route("Champions/{championId:int}/Details")]
        public IHttpActionResult GetChampionDetails(int championId)
        {
            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Champion>(championId);
            var details = this.DocumentSession.Load<ChampionToChampionDetailModelTransformer, ChampionDetailModel>(stringId);

            if (details == null)
                return NotFound();

            return Ok(details);
        }
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
