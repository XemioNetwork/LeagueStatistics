using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using LeagueStatistics.Server.Abstractions.Services;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using PortableLeagueAPI;
using PortableLeagueApi.Game.Models;
using PortableLeagueApi.Interfaces.Champion;
using PortableLeagueApi.Interfaces.Enums;
using PortableLeagueApi.Interfaces.Game;
using PortableLeagueApi.Interfaces.Static.Champion;
using PortableLeagueApi.Interfaces.Static.Item;
using PortableLeagueApi.Interfaces.Static.SummonerSpell;
using PortableLeagueApi.Interfaces.Summoner;
using PortableLeagueApi.Static.Extensions;

namespace LeagueStatistics.Server.Infrastructure.Implementations.Services
{
    public class LeagueService : ILeagueService
    {
        #region Fields
        private readonly LeagueApi _leagueApi;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueService" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="region">The region.</param>
        public LeagueService(string apiKey, RegionEnum region)
        {
            Condition.Requires(apiKey, "apiKey")
                .IsNotNullOrWhiteSpace();

            this.Logger = NullLogger.Instance;
            this._leagueApi = new LeagueApi(apiKey, region);
        }
        #endregion
        
        #region Implementation of ILeagueService
        /// <summary>
        /// Returns the recent matches from the specified summoner.
        /// </summary>
        /// <param name="summonerId">The summoner identifier.</param>
        public async Task<IEnumerable<Match>> GetRecentMatchesAsync(int summonerId)
        {
            Condition.Requires(summonerId, "summonerId")
                .IsNotLessOrEqual(0);

            try
            {
                IEnumerable<IGame> games = await this._leagueApi.Game.GetRecentGamesBySummonerIdAsync(summonerId);

                return games.Select(f =>
                {
                    var match = new Match
                    {
                        Id = string.Format("Matches/{0}", f.GameId),
                        CreationDate = f.CreateDate,
                        Length = TimeSpan.FromSeconds(f.Stats.TimePlayed),
                        GameMode = this.ConvertGameMode(f.GameMode),
                        GameType = this.ConvertGameType(f.GameType),
                        GameSubType = this.ConvertGameSubType(f.GameSubType),
                        MapId = string.Format("Maps/{0}", (int) f.Map),
                        Teams = new Collection<Team>
                        {
                            new Team
                            {
                                Color = ConvertTeam(f.TeamId),
                                Win = f.Stats.Win,
                                Players = new Collection<PlayerStats>
                                {
                                    new PlayerStats
                                    {
                                        ChampionId = string.Format("Champions/{0}", f.ChampionId),
                                        FirstSpellId = string.Format("Spells/{0}", f.SummonerSpells.First()),
                                        SecondSpellId = string.Format("Spells/{0}", f.SummonerSpells.Last()),
                                        DataLoaded = true,
                                        Win = f.Stats.Win,
                                        InfluencePointsEarned = f.IpEarned,
                                        TotalHeal = f.Stats.TotalHeal,
                                        TotalDamageDealt = f.Stats.TotalDamageDealt,
                                        TrueDamageDealt = f.Stats.TrueDamageDealtPlayer,
                                        PhysicalDamageDealt = f.Stats.PhysicalDamageDealtPlayer,
                                        MagicDamageDealt = f.Stats.MagicDamageDealtPlayer,
                                        TotalDamageDealtToChampions = f.Stats.TotalDamageDealtToChampions,
                                        TrueDamageDealtToChampions = f.Stats.TrueDamageDealtToChampions,
                                        PhysicalDamageDealtToChampions = f.Stats.PhysicalDamageDealtToChampions,
                                        MagicDamageDealtToChampions = f.Stats.MagicDamageDealtToChampions,
                                        TotalDamageTaken = f.Stats.TotalDamageTaken,
                                        TrueDamageTaken = f.Stats.TrueDamageTaken,
                                        PhysicalDamageTaken = f.Stats.PhysicalDamageTaken,
                                        MagicDamageTaken = f.Stats.MagicDamageTaken,
                                        Level = f.Stats.Level,
                                        GoldEarned = f.Stats.GoldEarned,
                                        GoldSpent = f.Stats.GoldSpent,
                                        Item1Id = this.ConvertItemId(f.Stats.ItemIds[0]),
                                        Item2Id = this.ConvertItemId(f.Stats.ItemIds[1]),
                                        Item3Id = this.ConvertItemId(f.Stats.ItemIds[2]),
                                        Item4Id = this.ConvertItemId(f.Stats.ItemIds[3]),
                                        Item5Id = this.ConvertItemId(f.Stats.ItemIds[4]),
                                        Item6Id = this.ConvertItemId(f.Stats.ItemIds[5]),
                                        SightWardsBought = f.Stats.SightWardsBought,
                                        WardsKilled = f.Stats.WardKilled,
                                        WardsPlaced = f.Stats.WardPlaced,
                                        ChampionsKilled = f.Stats.ChampionsKilled,
                                        Assists = f.Stats.Assists,
                                        Deaths = f.Stats.NumDeaths,
                                        MinionsKilled = f.Stats.MinionsKilled,
                                        NeutralMinionsKilledYourJungle = f.Stats.NeutralMinionsKilledYourJungle,
                                        NeutralMinionsKilledEnemyJungle = f.Stats.NeutralMinionsKilledEnemyJungle,
                                        LargestMultiKill = f.Stats.LargestMultiKill,
                                        SummonerId = string.Format("Summoners/{0}", summonerId)
                                    }
                                }
                            },
                            new Team
                            {
                                Color = GetOtherTeam(f.TeamId),
                                Win = !f.Stats.Win,
                                Players = new Collection<PlayerStats>()
                            }
                        }
                    };

                    foreach (var player in f.OtherPlayers)
                    {
                        Team team = match.Teams.First(d => d.Color == ConvertTeam(player.TeamId));
                        team.Players.Add(new PlayerStats
                        {
                            DataLoaded = false,
                            SummonerId = string.Format("Summoners/{0}", player.SummonerId),
                            ChampionId = string.Format("Champions/{0}", player.ChampionId)
                        });
                    }

                    return match;
                });
            }
            catch (Exception exception)
            {
                this.Logger.ErrorFormat(exception, "Unhandled exception in 'GetRecentMatchesAsync' for summoner '{0}'.", summonerId);
                return new List<Match>();
            }
        }
        /// <summary>
        /// Returns all champions.
        /// </summary>
        public async Task<IEnumerable<Champion>> GetChampionsAsync()
        {
            IChampionList champions = await this._leagueApi.Static.GetChampionsAsync(champData: ChampDataEnum.All);

            return champions.Data.Values.Select(f => new Champion
            {
                Id = string.Format("Champions/{0}", f.Id),
                Name = f.Name,
                ImageUrl = f.Image.GetUrlAsync().Result,
                Title = f.Title,
                MovementSpeed = f.Stats.Movespeed,
                AttackRange = f.Stats.Attackrange,
                Mana = f.Stats.MP,
                ManaPerLevel = f.Stats.Mpperlevel,
                ManaRegeneration = f.Stats.Mpregen,
                ManaRegenerationPerLevel = f.Stats.Mpregenperlevel,
                Health = f.Stats.HP,
                HealthPerLevel = f.Stats.Hpperlevel,
                HealthRegeneration = f.Stats.Hpregen,
                HealthRegenerationPerLevel = f.Stats.Hpregenperlevel,
                MagicResist = f.Stats.Spellblock,
                MagicResistPerLevel = f.Stats.Spellblockperlevel,
                Armor = f.Stats.Armor,
                ArmorPerLevel = f.Stats.Armorperlevel,
                AttackDamage = f.Stats.Attackdamage,
                AttackDamagePerLevel = f.Stats.Attackdamageperlevel,
                AttackSpeedOffset = f.Stats.Attackspeedoffset,
                AttackSpeedPerLevel = f.Stats.Attackspeedperlevel,
                Crit = f.Stats.Crit,
                CritPerLevel = f.Stats.Critperlevel
            });
        }
        /// <summary>
        /// Returns all summoner spells.
        /// </summary>
        public async Task<IEnumerable<Spell>> GetSpellsAsync()
        {
            ISummonerSpellList spells = await this._leagueApi.Static.GetSummonerSpellsAsync(itemData: SpellDataEnum.All);

            return spells.Data.Values.Select(f => new Spell
            {
                Id = string.Format("Spells/{0}", f.Id),
                Name = f.Name,
                Description = f.Description,
                Cooldown = f.Cooldown.FirstOrDefault(),
                ImageUrl = f.Image.GetUrlAsync().Result,
                SummonerLevel = f.SummonerLevel
            });
        }
        /// <summary>
        /// Returns all maps.
        /// </summary>
        public Task<IEnumerable<Map>> GetMapsAsync()
        {
            return Task.FromResult((IEnumerable<Map>) new List<Map>
            {
                new Map(1, "Summoner's Rift"),
                new Map(2, "Summoner's Rift"),
                new Map(3, "The Proving Grounds"),
                new Map(4, "Twisted Treeline"),
                new Map(8, "The Crystal Scar"),
                new Map(10, "Twisted Treeline"),
                new Map(12, "Howling Abyss")
            });
        }
        /// <summary>
        /// Returns all items.
        /// </summary>
        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            List<Map> maps = (await this.GetMapsAsync()).ToList();
            IItemList items = await this._leagueApi.Static.GetItemsAsync(ItemDataEnum.All);

            return items.Data.Values.Select(f =>
            {
                var item = new Item
                {
                    Id = string.Format("Items/{0}", f.Id),
                    Name = f.Name,
                    Description = f.Plaintext,
                    ImageUrl = f.Image.GetUrlAsync().Result,
                    StatsText = f.SanitizedDescription,
                    AvailableOnMaps = maps.Select(m => new ItemOnMap
                    {
                        MapId = m.Id,
                        IsAvailable = true
                    }).ToList(),
                    BuildsInto = f.Into.Select(i => string.Format("Items/{0}", i)).ToList(),
                    BuildsFrom = f.From.Select(i => string.Format("Items/{0}", i)).ToList(),
                    Purchasable = f.Gold.Purchasable,
                    SellPrice = f.Gold.Sell,
                    Price = f.Gold.Base,
                    TotalPrice = f.Gold.Total,
                    Tags = f.Tags
                };

                foreach (var map in f.Maps)
                {
                    item.AvailableOnMaps.First(m => m.MapId == string.Format("Maps/{0}", map.Key)).IsAvailable = map.Value;
                }

                return item;
            });
        }
        /// <summary>
        /// Returns the summoner with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task<Summoner> GetSummonerAsync(string name)
        {
            Condition.Requires(name, "name")
                .IsNotNullOrWhiteSpace();

            ISummoner summoner = await this._leagueApi.Summoner.GetSummonerByNameAsync(name);

            return new Summoner(summoner.SummonerId, summoner.Name);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Converts the teamId to the specified <see cref="Team"/>.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        private TeamColor ConvertTeam(int teamId)
        {
            switch (teamId)
            {
                case 100:
                    return TeamColor.Blue;
                case 200:
                    return TeamColor.Purple;
                default:
                    throw new ArgumentOutOfRangeException("teamId");
            }
        }
        /// <summary>
        /// Convers the teamId to the other <see cref="Team"/>.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        private TeamColor GetOtherTeam(int teamId)
        {
            return this.ConvertTeam(teamId) == TeamColor.Purple ? TeamColor.Blue : TeamColor.Purple;
        }
        /// <summary>
        /// Converts the game mode.
        /// </summary>
        /// <param name="gameMode">The game mode.</param>
        private GameMode ConvertGameMode(GameModeEnum gameMode)
        {
            switch (gameMode)
            {
                case GameModeEnum.Classic:
                    return GameMode.Classic;
                case GameModeEnum.Odin:
                    return GameMode.Odin;
                case GameModeEnum.Aram:
                    return GameMode.Aram;
                case GameModeEnum.Tutorial:
                    return GameMode.Tutorial;
                case GameModeEnum.OneForAll:
                    return GameMode.OneForAll;
                case GameModeEnum.FirstBlood:
                    return GameMode.FirstBlood;
                default:
                    throw new ArgumentOutOfRangeException("gameMode");
            }
        }
        /// <summary>
        /// Converts the type of the game.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        private GameType ConvertGameType(GameTypeEnum gameType)
        {
            switch (gameType)
            {
                case GameTypeEnum.CustomGame:
                    return GameType.Custom;
                case GameTypeEnum.MatchedGame:
                    return GameType.Matched;
                case GameTypeEnum.TutorialGame:
                    return GameType.Tutorial;
                default:
                    throw new ArgumentOutOfRangeException("gameType");
            }
        }
        /// <summary>
        /// Converts the type of the game sub.
        /// </summary>
        /// <param name="gameSubType">Type of the game sub.</param>
        private GameSubType ConvertGameSubType(GameSubTypeEnum gameSubType)
        {
            switch (gameSubType)
            {
                case GameSubTypeEnum.None:
                    return GameSubType.None;
                case GameSubTypeEnum.Normal:
                    return GameSubType.Normal;
                case GameSubTypeEnum.Bot:
                    return GameSubType.Bot;
                case GameSubTypeEnum.RankedSolo5X5:
                    return GameSubType.RankedSolo5v5;
                case GameSubTypeEnum.RankedPremade3X3:
                    return GameSubType.RankedTeam3v3;
                case GameSubTypeEnum.RankedPremade5X5:
                    return GameSubType.RankedTeam5v5;
                case GameSubTypeEnum.ODINUnranked:
                    return GameSubType.OdinUnranked;
                case GameSubTypeEnum.RankedTeam3X3:
                    return GameSubType.RankedTeam3v3;
                case GameSubTypeEnum.RankedTeam5X5:
                    return GameSubType.RankedTeam5v5;
                case GameSubTypeEnum.Normal3X3:
                    return GameSubType.Normal3v3;
                case GameSubTypeEnum.Bot3X3:
                    return GameSubType.Bot3v3;
                case GameSubTypeEnum.ARAMUnranked5X5:
                    return GameSubType.AramUnranked5v5;
                case GameSubTypeEnum.OneForAll5X5:
                    return GameSubType.OneForAll5v5;
                case GameSubTypeEnum.FirstBlood1X1:
                    return GameSubType.FirstBlood1v1;
                case GameSubTypeEnum.FirstBlood2X2:
                    return GameSubType.FirstBlood2v2;
                case GameSubTypeEnum.SR6X6:
                    return GameSubType.Hexakill;
                case GameSubTypeEnum.CAP5X5:
                    return GameSubType.TeamBuilder;
                case GameSubTypeEnum.Urf:
                    return GameSubType.Urf;
                case GameSubTypeEnum.UrfBot:
                    return GameSubType.UrfBot;
                default:
                    throw new ArgumentOutOfRangeException("gameSubType");
            }
        }
        /// <summary>
        /// Creates the item string identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        private string ConvertItemId(int itemId)
        {
            if (itemId == 0)
                return null;

            return "Items/" + itemId;
        }
        #endregion
    }
}
