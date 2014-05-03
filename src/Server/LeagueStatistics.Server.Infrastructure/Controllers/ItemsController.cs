using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Returns all items.
        /// </summary>
        [Route("Items")]
        public IHttpActionResult GetItems()
        {
            var items = this.DocumentSession.Query<Item>()
                .TransformWith<ItemToItemModelTransformer, ItemModel>()
                .OrderBy(f => f.Id)
                .Take(1000)
                .ToList();

            return Ok(items);
        }

        /// <summary>
        /// Returns the item with the specified <paramref name="itemId"/>.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        [Route("Items/{itemId:int}")]
        public IHttpActionResult GetItem(int itemId)
        {
            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Item>(itemId);
            var item = this.DocumentSession.Load<ItemToItemModelTransformer, ItemModel>(stringId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }
        /// <summary>
        /// Returns the details for the item with the specified <paramref name="itemId"/>.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        [Route("Items/{itemId:int}/Details")]
        public IHttpActionResult GetItemDetails(int itemId)
        {
            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Item>(itemId);
            var item = this.DocumentSession.Load<ItemToItemDetailModelTransformer, ItemDetailModel>(stringId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }
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
