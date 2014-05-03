using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace LeagueStatistics.Server.Infrastructure.Windsor
{
    public class ArrayFacility : IFacility
    {
        #region Implementation of IFacility
        /// <summary>
        /// Initializes this facility.
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="facilityConfig"></param>
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new ArrayResolver(kernel));
        }
        /// <summary>
        /// Terminates this facility.
        /// </summary>
        public void Terminate()
        {
        }
        #endregion
    }
}
