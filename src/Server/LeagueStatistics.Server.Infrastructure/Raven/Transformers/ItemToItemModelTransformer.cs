using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;
using LeagueStatistics.Shared.Models;
using Raven.Client.Indexes;

namespace LeagueStatistics.Server.Infrastructure.Raven.Transformers
{
    public class ItemToItemModelTransformer : AbstractTransformerCreationTask<Item>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemToItemModelTransformer"/> class.
        /// </summary>
        public ItemToItemModelTransformer()
        {
            this.TransformResults = items => from item in items
                                             select new ItemModel
                                             {
                                                 Id = int.Parse(item.Id.Split('/').Last()),
                                                 Description = item.Description,
                                                 ImageUrl = item.ImageUrl,
                                                 Name = item.Name,
                                                 StatsText = item.StatsText,
                                                 Tags = item.Tags
                                             };
        }
        #endregion

        #region Overrides of AbstractTransformerCreationTask
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string TransformerName
        {
            get { return "Item/ToItemModel"; }
        }
        #endregion
    }
}
