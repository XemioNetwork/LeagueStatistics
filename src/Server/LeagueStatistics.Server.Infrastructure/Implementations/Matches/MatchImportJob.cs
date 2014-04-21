using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            using (IDocumentSession documentSession = this._documentStore.OpenSession())
            {
                IEnumerable<Summoner> summoners = this.GetSummonersToLoadMatchesFrom(documentSession);

                foreach (var summoner in summoners)
                {
                    IEnumerable<Match> matches = this._leagueService.GetRecentMatchesAsync(summoner.Id.GetIntId()).Result;
                    foreach (var match in matches)
                    {
                        documentSession.Store(match);
                    }

                    summoner.NextMatchImportDate = DateTimeOffset.UtcNow.AddMinutes(10);
                }

                documentSession.SaveChanges();
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
