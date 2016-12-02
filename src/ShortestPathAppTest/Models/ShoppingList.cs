using System;
using System.Collections.Generic;

namespace QuickShopper.Models
{
    public class ShoppingList
    {
        public long UserId { get; set; }
        public long Id { get; set; }
        public virtual IList<Item> Items { get; set; }
    }
}
