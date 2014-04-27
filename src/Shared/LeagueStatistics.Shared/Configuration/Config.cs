using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Configuration
{
    public static class Config
    {
        /// <summary>
        /// Returns the config value with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public static string GetValue(string name)
        {
            return ConfigurationManager.AppSettings["LeagueStatistics/" + name];
        }
    }
}
