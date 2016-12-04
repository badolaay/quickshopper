using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickShopper.Data;

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

        private readonly ApplicationDbContext _context;

        public ShopingListItems(ApplicationDbContext context)
        {
            _context = context;
        }

        private ShopingListItems()
        {
        }

        public void AddToList(long itemId, string user)
        {
            //check if item already exists in the corresponding list for the user
            var listItem = _context.ShopingListItems.SingleOrDefault(c => c.UserId == user && c.ItemId == itemId);
            if (listItem == null)
            {
                // Create a new shoppinglist item if no shoppinglist item exists.                 
                listItem = new ShopingListItems
                {
                    ItemId = itemId,
                    UserId = user,
                    Item = _context.Item.SingleOrDefault(item => item.Id == itemId),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _context.ShopingListItems.Add(listItem);
            }
            else
            {
                // If the item exists,increase quantity
                listItem.Quantity++;
            }
            _context.SaveChanges();
        }

        public long GetQuantity(string userid)
        {
            long quantity = 0;
            var queryable = _context.ShopingListItems.Where(c => c.UserId == userid).Select(c => c.Quantity);
            foreach (long l in queryable)
            {
                quantity += l;
            }
            return quantity;
        }
    }
}

