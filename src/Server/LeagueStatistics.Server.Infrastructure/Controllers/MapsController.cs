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
using LeagueStatistics.Server.Infrastructure.Raven.Transformers;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Controllers
{
    public class MapsController : BaseController
    {
        #region Fields
        private readonly ILeagueService _leagueService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MapsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="leagueService">The league service.</param>
        public MapsController(IDocumentSession documentSession, ILeagueService leagueService) 
            : base(documentSession)
        {
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this._leagueService = leagueService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all maps.
        /// </summary>
        [Route("Maps")]
        public IHttpActionResult GetMaps()
        {
            var maps = this.DocumentSession.Query<Map>()
                .TransformWith<MapToMapModelTransformer, MapModel>()
                .OrderBy(f => f.Id)
                .ToList();

            return Ok(maps);
        }
        /// <summary>
        /// Returns the map with the specified <paramref name="mapId"/>.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        [Route("Maps/{mapId:int}")]
        public IHttpActionResult GetMap(int mapId)
        {
            Condition.Requires(mapId, "mapId")
                .IsGreaterThan(0);

            var stringId = this.DocumentSession.Advanced.GetStringIdFor<Map>(mapId);
            var map = this.DocumentSession.Load<MapToMapModelTransformer, MapModel>(stringId);

            if (map == null)
                return NotFound();

            return Ok(map);
        }
        /// <summary>
        /// Updates all map data.
        /// </summary>
        [Route("Maps/Update")]
        [RequiresAuthentication]
        public async Task<IHttpActionResult> GetUpdateMaps()
        {
            IEnumerable<Map> maps = await this._leagueService.GetMapsAsync();
            foreach (var map in maps)
            {
                this.DocumentSession.Store(map);
            }

            return Ok();
        }
        #endregion
    }
}
