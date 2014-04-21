using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Controllers
{
    public class BaseController : ApiController
    {
        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore
        {
            get { return this.DocumentSession.Advanced.DocumentStore; }
        }
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether an exception occured.
        /// </summary>
        public bool ExceptionOccured { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public BaseController(IDocumentSession documentSession)
        {
            Condition.Requires(documentSession, "documentSession")
                .IsNotNull();

            this.DocumentSession = documentSession;
        }
        #endregion

        #region Overrides of ApiController
        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var response = await base.ExecuteAsync(controllerContext, cancellationToken);

            //We get this set by the "HandleExceptionAttribute" so we know that we should not save the changes
            if (this.ExceptionOccured == false)
            {
                using (this.DocumentSession)
                {
                    this.DocumentSession.SaveChanges();
                }
            }

            return response;
        }
        #endregion
    }
}
