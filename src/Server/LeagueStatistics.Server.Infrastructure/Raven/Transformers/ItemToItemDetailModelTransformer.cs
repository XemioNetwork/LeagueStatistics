using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Server.Infrastructure.Extensions;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Transformers
{
    public class ItemToItemDetailModelTransformer : AbstractTransformerCreationTask<Item>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemToItemDetailModelTransformer"/> class.
        /// </summary>
        public ItemToItemDetailModelTransformer()
        {
            this.TransformResults = items => from item in items
                                             select new ItemDetailModel
                                             {
                                                 Id = int.Parse(item.Id.Split('/').Last()),
                                                 Name = item.Name,
                                                 Description = item.Description,
                                                 ImageUrl = item.ImageUrl,
                                                 StatsText = item.StatsText,
                                                 AvailableOnMaps = item.AvailableOnMaps.Select(f => new ItemOnMapModel
                                                     {
                                                         IsAvailable = f.IsAvailable,
                                                         MapId = int.Parse(f.MapId.Split('/').Last()),
                                                         MapName = LoadDocument<Map>(f.MapId).Name
                                                     }).ToList(),
                                                 BuildsFrom = item.BuildsFrom.Select(f => new ItemModel
                                                     {
                                                         Id = int.Parse(f.Split('/').Last()),
                                                         Name = LoadDocument<Item>(f).Name,
                                                         Description = LoadDocument<Item>(f).Description,
                                                         ImageUrl = LoadDocument<Item>(f).ImageUrl,
                                                         StatsText = LoadDocument<Item>(f).StatsText,
                                                         Tags = LoadDocument<Item>(f).Tags.ToList()
                                                     }).ToList(),
                                                 BuildsInto = item.BuildsInto.Select(f => new ItemModel
                                                     {
                                                         Id = int.Parse(f.Split('/').Last()),
                                                         Name = LoadDocument<Item>(f).Name,
                                                         Description = LoadDocument<Item>(f).Description,
                                                         ImageUrl = LoadDocument<Item>(f).ImageUrl,
                                                         StatsText = LoadDocument<Item>(f).StatsText,
                                                         Tags = LoadDocument<Item>(f).Tags.ToList()
                                                     }).ToList(),
                                                 Price = item.Price,
                                                 SellPrice = item.SellPrice,
                                                 Tags = item.Tags.ToList(),
                                                 Purchasable = item.Purchasable,
                                                 TotalPrice = item.TotalPrice
                                             };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string TransformerName
        {
            get { return "Item/ToItemDetailModel"; }
        }
        #endregion
    }
}
