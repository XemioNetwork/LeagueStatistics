using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ItemsController : BaseController
    {
        #region Fields
        private readonly ILeagueService _leagueService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="leagueService">The league service.</param>
        public ItemsController(IDocumentSession documentSession, ILeagueService leagueService) 
            : base(documentSession)
        {
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this._leagueService = leagueService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates all item data.
        /// </summary>
        [Route("Items/Update")]
        [RequiresAuthentication]
        public async Task<IHttpActionResult> GetUpdateItems()
        {
            IEnumerable<Item> items = await this._leagueService.GetItemsAsync();
            foreach (var item in items)
            {
                this.DocumentSession.Store(item);
            }

            return Ok();
        }
        #endregion
    }
}
