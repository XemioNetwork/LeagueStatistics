using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueStatistics.Shared.Entities;

namespace LeagueStatistics.Shared.Models
{
    public class ItemDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string StatsText { get; set; }
        public ICollection<ItemOnMapModel> AvailableOnMaps { get; set; }
        public ICollection<ItemModel> BuildsInto { get; set; }
        public ICollection<ItemModel> BuildsFrom { get; set; }
        public bool Purchasable { get; set; }
        public int SellPrice { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public ICollection<string> Tags { get; set; }
    }

    public class ItemOnMapModel
    {
        public int MapId { get; set; }
        public string MapName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
