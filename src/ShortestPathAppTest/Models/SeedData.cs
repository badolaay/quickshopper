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
                using (var transaction = context.Database.BeginTransaction())
                {
                    //delete every item at startup
                    if (context.Item.Any())
                    {
                        foreach (Item item in context.Item)
                        {
                            context.Remove(item);
                        }
                    }
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Item] ON");
                    context.Item.AddRange(
                        new Item
                        {
                            Id = 1,
                            Name = "Apples",
                            Price = 7.99D,
                            Discount = 10.00D,
                            Category = "Groceries",
                            ImagePath = ""

                        },
                        new Item
                        {
                            Id = 2,
                            Name = "Oranges",
                            Price = 9.99D,
                            Discount = 5.00D,
                            Category = "Groceries",
                            ImagePath = ""
                        }, new Item
                        {
                            Id = 3,
                            Name = "Tomatoes",
                            Price = 7.99D,
                            Discount = 10.00D,
                            Category = "Groceries",
                            ImagePath = ""
                        }, new Item
                        {
                            Id = 4,
                            Name = "Carrots",
                            Price = 8.99D,
                            Discount = 10.00D,
                            Category = "Groceries",
                            ImagePath = ""
                        }, new Item
                        {
                            Id = 5,
                            Name = "Onions",
                            Price = 2.99D,
                            Discount = 0.00D,
                            Category = "Groceries",
                            ImagePath = ""
                        }
                    );
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Item] OFF");
                    transaction.Commit();
                }
            }
        }
    }
}