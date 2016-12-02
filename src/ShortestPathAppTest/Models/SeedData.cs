using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickShopper.Data;

namespace QuickShopper.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Item.Any())
                {
                    return;   // DB has been seeded
                }

                context.Item.AddRange(
                     new Item
                     {
                         Name = "Apples",
                         Price = 7.99D,
                         Discount = 10.00D,
                         Category = "Groceries",

                     },
                     new Item
                     {
                         Name = "Oranges",
                         Price = 9.99D,
                         Discount = 5.00D,
                         Category = "Groceries",

                     }, new Item
                     {
                         Name = "Tomatoes",
                         Price = 7.99D,
                         Discount = 10.00D,
                         Category = "Groceries",

                     }, new Item
                     {
                         Name = "Carrots",
                         Price = 8.99D,
                         Discount = 10.00D,
                         Category = "Groceries",

                     }
                );
                context.SaveChanges();
            }
        }
    }
}
