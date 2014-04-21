using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Server.Infrastructure;
using Topshelf;

namespace LeagueStatistics.Server.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<LeagueStatisticsService>(s =>
                {
                    s.ConstructUsing(f => new LeagueStatisticsService());
                    s.WhenStarted(f => f.Start());
                    s.WhenStopped(f => f.Stop());
                });

                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("The HTTP API for League Statistics");
                x.SetDisplayName("League Statistics HTTP API");
                x.SetServiceName("LeagueStatisticsHTTPAPI");
            });
        }
    }
}
