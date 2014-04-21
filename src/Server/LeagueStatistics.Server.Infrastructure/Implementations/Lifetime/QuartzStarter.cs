using Castle.Core;
using CuttingEdge.Conditions;
using Quartz;

namespace LeagueStatistics.Server.Infrastructure.Implementations.Lifetime
{
    public class QuartzStarter : IStartable
    {
        #region Fields
        private readonly IScheduler _scheduler;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzStarter"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public QuartzStarter(IScheduler scheduler)
        {
            Condition.Requires(scheduler, "scheduler")
                .IsNotNull();

            this._scheduler = scheduler;
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts the scheduler.
        /// </summary>
        public void Start()
        {
            this._scheduler.Start();
        }
        /// <summary>
        /// Stops the scheduler.
        /// </summary>
        public void Stop()
        {
            this._scheduler.Shutdown(true);
        }
        #endregion
    }
}