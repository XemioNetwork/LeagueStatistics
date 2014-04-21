using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LeagueStatistics.Server.Infrastructure.Controllers;

namespace LeagueStatistics.Server.Infrastructure.Windsor
{
    public class ControllerInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<BaseController>()
                .LifestyleTransient()
                .WithServiceSelf());
        }
        #endregion
    }
}
