using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Models;

namespace LeagueStatistics.Shared.Entities
{
    public class Item : AggregateRoot
    {
        public Item()
        {
            this.AvailableOnMaps = new Collection<ItemOnMap>();
            this.BuildsInto = new Collection<string>();
            this.BuildsFrom = new Collection<string>();
            this.Tags = new Collection<string>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string StatsText { get; set; }
        public ICollection<ItemOnMap> AvailableOnMaps { get; set; }
        public ICollection<string> BuildsInto { get; set; }
        public ICollection<string> BuildsFrom { get; set; }
        public bool Purchasable { get; set; }
        public int SellPrice { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
