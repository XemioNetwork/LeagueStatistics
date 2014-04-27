using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Configuration;
using Microsoft.Owin.Hosting;
using Raven.Abstractions.Extensions;

namespace LeagueStatistics.Server.Infrastructure
{
    public class LeagueStatisticsService
    {
        #region Fields
        private IDisposable _service;
        #endregion

        #region Methods
        /// <summary>
        /// Starts the league statistics service.
        /// </summary>
        public void Start()
        {
            string addresses = Config.GetValue("Addresses");

            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(addresses.Split('|'));

            this._service = WebApp.Start<Startup>(startOptions);
        }
        /// <summary>
        /// Stops the league statistics service.
        /// </summary>
        public void Stop()
        {
            if (this._service != null)
                this._service.Dispose();
        }
        #endregion
    }
}
