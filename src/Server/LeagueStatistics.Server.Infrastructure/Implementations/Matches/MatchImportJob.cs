using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Abstractions.Services;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Shared.Entities;
using Quartz;
using Raven.Client;
using Raven.Client.Exceptions;

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
                    //Load the next summoners
                    List<Summoner> summoners = this.GetSummonersToLoadMatchesFrom(documentSession);
                    foreach (var summoner in summoners)
                    {
                        //Get the matches that currently dont exist
                        IEnumerable<Match> matches =
                            from match in this._leagueService.GetRecentMatchesAsync(summoner.Id.GetIntId()).Result
                            where this._documentStore.DatabaseCommands.DocumentExists(match.Id) == false
                            select match;

                        foreach (var match in matches)
                        {
                            //Store these matches
                            try
                            {
                                documentSession.Store(match);
                            }
                            catch (NonUniqueObjectException)
                            {
                                //Just ignore if we import a match twice in one run
                            }
                            this.Logger.DebugFormat("Imported match '{0}'.", match.Id);

                            //Store the other players as "NeededSummonerMatch"es
                            foreach (var player in match.Teams.SelectMany(f => f.Players).Where(f => f.DataLoaded == false))
                            {
                                documentSession.Store(new NeededSummonerMatch
                                {
                                    Date = DateTimeOffset.Now,
                                    SummonerId = player.SummonerId,
                                    MatchId = match.Id,
                                });
                            }
                        }

                        //Update the summoner
                        summoner.LastMatchImportDate = DateTimeOffset.UtcNow;
                        summoner.NextMatchImportDate = summoner.LastMatchImportDate.AddMinutes(30);
                    }

                    //Now load the needed matches
                    List<NeededSummonerMatch> neededMatches = this.GetAdditionalMatchesFrom(documentSession, summoners.Count);
                    foreach (var neededMatch in neededMatches)
                    {
                        //Get the matches from the player
                        IEnumerable<Match> importedMatches = this._leagueService.GetRecentMatchesAsync(neededMatch.SummonerId.GetIntId()).Result;
                        Match importedMatch = importedMatches.FirstOrDefault(f => f.Id == neededMatch.MatchId);

                        //If the needed match doesnt exist we cant do anything
                        if (importedMatch != null)
                        {
                            PlayerStats playerStats = importedMatch.Teams.SelectMany(f => f.Players).First(f => f.DataLoaded);
                            Match loadedMatch = documentSession.Load<Match>(importedMatch.Id);

                            Team team = loadedMatch.Teams.First(f => f.Players.Any(d => d.SummonerId == playerStats.SummonerId));

                            team.Players.Remove(team.Players.First(f => f.SummonerId == playerStats.SummonerId));
                            team.Players.Add(playerStats);
                        }

                        documentSession.Delete(neededMatch);
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
        private List<Summoner> GetSummonersToLoadMatchesFrom(IDocumentSession documentSession)
        {
            return documentSession.Query<Summoner>()
                .Where(f => f.NextMatchImportDate <= DateTimeOffset.UtcNow)
                .Take(10)
                .ToList();
        }
        /// <summary>
        /// Returns the additional matches.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="count">The count.</param>
        private List<NeededSummonerMatch> GetAdditionalMatchesFrom(IDocumentSession documentStore, int count)
        {
            return documentStore.Query<NeededSummonerMatch>()
                .OrderBy(f => f.Date)
                .Take(10 - count)
                .ToList();
        }
        #endregion
    }
}
