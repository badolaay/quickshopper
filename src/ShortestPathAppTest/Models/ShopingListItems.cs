using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickShopper.Models
{
    public class ShopingListItems
    {
        public long Id { get; set; }
        public String UserId { get; set; }
        public long Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public long ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
