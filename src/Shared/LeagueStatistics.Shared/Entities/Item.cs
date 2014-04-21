using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueStatistics.Shared.Entities
{
    public class Item : AggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
