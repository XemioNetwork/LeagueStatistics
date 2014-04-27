using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Abstractions.Services;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Shared.Entities;
using Quartz;
using Raven.Client;

namespace LeagueStatistics.Server.Infrastructure.Implementations.Matches
{
    [DisallowConcurrentExecution]
    public class MatchImportJob : IJob
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        private readonly ILeagueService _leagueService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchImportJob"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="leagueService">The league service.</param>
        public MatchImportJob(IDocumentStore documentStore, ILeagueService leagueService)
        {
            Condition.Requires(documentStore, "documentStore")
                .IsNotNull();
            Condition.Requires(leagueService, "leagueService")
                .IsNotNull();

            this.Logger = NullLogger.Instance;

            this._documentStore = documentStore;
            this._leagueService = leagueService;
        }
        #endregion

        #region Implementation of IJob
        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                using (IDocumentSession documentSession = this._documentStore.OpenSession())
                {
                    IEnumerable<Summoner> summoners = this.GetSummonersToLoadMatchesFrom(documentSession);

                    foreach (var summoner in summoners)
                    {
                        IEnumerable<Match> matches =
                            from match in this._leagueService.GetRecentMatchesAsync(summoner.Id.GetIntId()).Result
                            where this._documentStore.DatabaseCommands.DocumentExists(match.Id) == false
                            select match;

                        foreach (var match in matches)
                        {
                            documentSession.Store(match);
                            this.Logger.DebugFormat("Imported match '{0}'.", match.Id);
                        }

                        summoner.LastMatchImportDate = DateTimeOffset.UtcNow;
                        summoner.NextMatchImportDate = summoner.LastMatchImportDate.AddMinutes(30);
                    }

                    documentSession.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                this.Logger.ErrorFormat(exception, "Unhandled exception occured while executing the match import.");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the summoners to load it's matches.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        private IEnumerable<Summoner> GetSummonersToLoadMatchesFrom(IDocumentSession documentSession)
        {
            return documentSession.Query<Summoner>()
                .Where(f => f.NextMatchImportDate <= DateTimeOffset.UtcNow)
                .Take(10)
                .ToList();
        }
        #endregion
    }
}
