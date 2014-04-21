using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Castle.Facilities.Logging;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using LeagueStatistics.Server.Infrastructure.Filters;
using LeagueStatistics.Server.Infrastructure.Windsor;
using Owin;

namespace LeagueStatistics.Server.Infrastructure
{
    public class Startup
    {
        #region Methods
        /// <summary>
        /// Configurations the webservices.
        /// </summary>
        /// <param name="appBuilder">The app builder.</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            this.ConfigureCrossOriginRequests(config);
            this.ConfigureWindsor(config);
            this.ConfigureFilters(config);
            this.ConfigureRoutes(config);

            appBuilder.UseWebApi(config);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures the cross origin requests.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureCrossOriginRequests(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }
        /// <summary>
        /// Configures the castle windsor IoC container.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureWindsor(HttpConfiguration config)
        {
            var container = new WindsorContainer();

            container.AddFacility<LoggingFacility>(f => f.UseNLog());
            container.AddFacility<StartableFacility>();

            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

            container.Install(FromAssembly.This());

            config.DependencyResolver = new WindsorResolver(container);
        }
        /// <summary>
        /// Configures the filters.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new HandleExceptionAttribute());
        }
        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
        #endregion
    }
}
