using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;

namespace LeagueStatistics.Server.Abstractions.Services
{
    public interface ILeagueService : IService
    {
        /// <summary>
        /// Returns the recent matches from the specified summoner.
        /// </summary>
        /// <param name="summonerId">The summoner identifier.</param>
        Task<IEnumerable<Match>> GetRecentMatchesAsync(int summonerId);
        /// <summary>
        /// Returns all champions.
        /// </summary>
        Task<IEnumerable<Champion>> GetChampionsAsync();
        /// <summary>
        /// Returns all summoner spells.
        /// </summary>
        Task<IEnumerable<Spell>> GetSpellsAsync();
        /// <summary>
        /// Returns all maps.
        /// </summary>
        Task<IEnumerable<Map>> GetMapsAsync();
        /// <summary>
        /// Returns all items.
        /// </summary>
        Task<IEnumerable<Item>> GetItemsAsync();
        /// <summary>
        /// Returns the summoner with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        Task<Summoner> GetSummonerAsync(string name);
    }
}
