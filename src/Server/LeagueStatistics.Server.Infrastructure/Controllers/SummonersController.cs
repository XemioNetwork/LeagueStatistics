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
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Server.Infrastructure.Filters;
using LeagueStatistics.Shared.Entities;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Controllers
{
    public class SummonersController : BaseController
    {
        #region Fields
        private readonly ILeagueService _leagueService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SummonersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="leagueService">The league service.</param>
        public SummonersController(IDocumentSession documentSession, ILeagueService leagueService) 
            : base(documentSession)
        {
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this._leagueService = leagueService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a list of all summoners.
        /// </summary>
        [Route("Summoners")]
        public IHttpActionResult GetSummoners(int page, int pageSize)
        {
            var summoners = this.DocumentSession.Query<Summoner>()
                .OrderBy(f => f.Id)
                .Page(page, pageSize)
                .ToList();

            return this.Ok(summoners);
        }
        /// <summary>
        /// Patches the summoners.
        /// </summary>
        [Route("Summoners/Add/{username}")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetAddSummoner(string username)
        {
            Summoner summoner = await this._leagueService.GetSummonerAsync(username);
            this.DocumentSession.Store(summoner);

            return Request.CreateResponse(HttpStatusCode.Created, summoner);
        }
        #endregion
    }
}
