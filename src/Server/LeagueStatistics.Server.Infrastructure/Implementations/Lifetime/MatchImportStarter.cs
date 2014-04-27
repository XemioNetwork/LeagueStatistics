using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Infrastructure.Implementations.Matches;
using Quartz;

namespace LeagueStatistics.Server.Infrastructure.Implementations.Lifetime
{
    public class MatchImportStarter : IStartable
    {
        #region Fields
        private readonly IScheduler _scheduler;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchImportStarter"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public MatchImportStarter(IScheduler scheduler)
        {
            Condition.Requires(scheduler, "scheduler")
                .IsNotNull();

            this._scheduler = scheduler;
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts to import matches.
        /// </summary>
        public void Start()
        {
            var job = JobBuilder.Create<MatchImportJob>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(f => f
                    .WithIntervalInSeconds(15)
                    .RepeatForever())
                .StartNow()
                .Build();

            this._scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// Stops to import matches.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}
