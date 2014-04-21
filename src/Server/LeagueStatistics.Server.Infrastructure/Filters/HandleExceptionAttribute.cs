using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Castle.Core.Logging;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Server.Infrastructure.Controllers;
using Raven.Abstractions.Extensions;

namespace LeagueStatistics.Server.Infrastructure.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        #region Overrides of ExceptionFilterAttribute
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            ILogger logger = this.GetLogger(context);
            logger.Error("Unhandled exception occured.", context.Exception);

            if (context.ActionContext.ControllerContext.Controller is BaseController)
            {
                var controller = (BaseController) context.ActionContext.ControllerContext.Controller;
                controller.ExceptionOccured = true;
            }
#if DEBUG
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.ToString())
            };
#else
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
#endif
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns a <see cref="ILogger"/> for the current <see cref="HttpActionExecutedContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        private ILogger GetLogger(HttpActionExecutedContext context)
        {
            ILoggerFactory loggerFactory = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<ILoggerFactory>();
            string loggerName = context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

            return loggerFactory.Create(loggerName);
        }
        #endregion
    }
}