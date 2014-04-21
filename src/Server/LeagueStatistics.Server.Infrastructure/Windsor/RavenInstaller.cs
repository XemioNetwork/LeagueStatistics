﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;

namespace LeagueStatistics.Server.Infrastructure.Windsor
{
    public class RavenInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IDocumentStore>().Instance(this.GetDocumentStore()).LifestyleSingleton(),
                Component.For<IDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenSession()).LifestyleScoped()
            );
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the document store.
        /// </summary>
        private IDocumentStore GetDocumentStore()
        {
            return new DocumentStore
            {
                ConnectionStringName = "LeagueStatistics/RavenDB",
                DefaultDatabase = "LeagueStatistics"
            }.Initialize();
        }
        #endregion
    }
}
