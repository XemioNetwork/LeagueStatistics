using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using LeagueStatistics.Server.Infrastructure.Extensions;
using Raven.Client;
using Raven.Json.Linq;

namespace LeagueStatistics.Server.Infrastructure.Filters
{
    public class RequiresAuthenticationAttribute : AuthorizeAttribute
    {
        #region Overrides of AuthorizeAttribute
        /// <summary>
        /// Indicates whether the specified control is authorized.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override bool IsAuthorized(HttpActionContext context)
        {
            if (this.IsAdminKeyPresent(context) == false)
                return false;

            string adminKey = this.GetAdminKey(context);
            string databaseAdminKey = this.GetDatabaseAdminKey(context);

            return string.Equals(adminKey, databaseAdminKey, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the admin key is present.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool IsAdminKeyPresent(HttpActionContext context)
        {
            var arguments = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
            return arguments.AllKeys.Contains("admin_key");
        }
        /// <summary>
        /// Returns the admin key.
        /// </summary>
        /// <param name="context">The context.</param>
        private string GetAdminKey(HttpActionContext context)
        {
            var arguments = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
            return arguments["admin_key"];
        }
        /// <summary>
        /// Gets the database admin key.
        /// </summary>
        /// <param name="context">The context.</param>
        private string GetDatabaseAdminKey(HttpActionContext context)
        {
            var session = context.ControllerContext.Configuration.DependencyResolver.GetService<IDocumentSession>();

            var databaseInstance = session.Load<RavenJObject>("LeagueStatistics/Administration");
            return databaseInstance.Value<string>("Key");
        }
        #endregion
    }
}
