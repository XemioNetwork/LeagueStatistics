using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
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
        public IHttpActionResult GetSummoners(int page = 1, int pageSize = 50)
        {
            Condition.Requires(page, "page")
                .IsGreaterThan(0);
            Condition.Requires(pageSize, "pageSize")
                .IsGreaterThan(0);

            var summoners = this.DocumentSession.Query<Summoner>()
                .TransformWith<SummonerToSummonerModelTransformer, SummonerModel>()
                .OrderBy(f => f.Id)
                .Paging(page, pageSize)
                .ToList();

            return Ok(summoners);
        }
        /// <summary>
        /// Gets the summoner.
        /// </summary>
        /// <param name="summonerId">The summoner identifier.</param>
        [Route("Summoners/{summonerId:int}")]
        public IHttpActionResult GetSummoner(int summonerId)
        {
            Condition.Requires(summonerId, "summonerId")
                .IsGreaterThan(0);

            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Summoner>(summonerId);
            var summoner = this.DocumentSession.Load<SummonerToSummonerModelTransformer, SummonerModel>(stringId);

            if (summoner == null)
                return NotFound();

            return Ok(summoner);
        }
        /// <summary>
        /// Patches the summoners.
        /// </summary>
        [Route("Summoners/Add/{username}")]
        [RequiresAuthentication]
        public async Task<IHttpActionResult> GetAddSummoner(string username)
        {
            Condition.Requires(username, "username")
                .IsNotNullOrWhiteSpace();

            Summoner summoner = await this._leagueService.GetSummonerAsync(username);
            this.DocumentSession.Store(summoner);

            return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created));
        }
        #endregion
    }
}
