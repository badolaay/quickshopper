using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuickShopper.Data;
using QuickShopper.Models;
using QuickShopper.TSP;

namespace QuickShopper.Controllers
{
    [Authorize]
    public class ShopingListItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopingListItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShopingListItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShopingListItems.Include(s => s.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShopingListItems/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingListItems = await _context.ShopingListItems.SingleOrDefaultAsync(m => m.Id == id);
            if (shopingListItems == null)
            {
                return NotFound();
            }

            return View(shopingListItems);
        }

        // GET: ShopingListItems/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");
            return View();
        }

        // POST: ShopingListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,ItemId,Quantity,UserId")] ShopingListItems shopingListItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopingListItems);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shopingListItems.ItemId);
            return View(shopingListItems);
        }

        // GET: ShopingListItems/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingListItems = await _context.ShopingListItems.SingleOrDefaultAsync(m => m.Id == id);
            if (shopingListItems == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shopingListItems.ItemId);
            return View(shopingListItems);
        }

        // POST: ShopingListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DateCreated,ItemId,Quantity,UserId")] ShopingListItems shopingListItems)
        {
            if (id != shopingListItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopingListItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopingListItemsExists(shopingListItems.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shopingListItems.ItemId);
            return View(shopingListItems);
        }

        // GET: ShopingListItems/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingListItems = await _context.ShopingListItems.SingleOrDefaultAsync(m => m.Id == id);
            if (shopingListItems == null)
            {
                return NotFound();
            }

            return View(shopingListItems);
        }

        // POST: ShopingListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var shopingListItems = await _context.ShopingListItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.ShopingListItems.Remove(shopingListItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ShopingListItemsExists(long id)
        {
            return _context.ShopingListItems.Any(e => e.Id == id);
        }

        // GET: Optimized path
        public async Task<IActionResult> Path()
        {
            var queryResultItems = _context.ShopingListItems.Where(list => list.UserId == User.Identity.Name).Select(list => list.Item);

            IDictionary<long, Item> itemIdMapping = new Dictionary<long, Item>(queryResultItems.Count());

            int[] itemIds = new int[queryResultItems.Count() + 1];
            itemIds[0] = 0;//the store entry point
            int i = 1;
            foreach (Item item in queryResultItems)
            {

                itemIds[i] = (int)item.Id;
                i++;
                itemIdMapping.Add(item.Id, item);
            }
            IList<Item> items = new List<Item>(queryResultItems.Count());
            //var queryResult =
            //    _context.ShopingListItems.Where(list => list.UserId == User.Identity.Name).Select(list => list.ItemId);
            //int[] itemIds = new int[queryResult.Count() + 1];
            //itemIds[0] = 0;//the store entry point
            //int i = 1;
            //foreach (var l in queryResult)
            //{
            //    itemIds[i] = (int)l;
            //    i++;
            //}
            ItemsForPath itemsForPath = new ItemsForPath { ItemIds = itemIds };

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string body = JsonConvert.SerializeObject(itemsForPath);
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = await client.PostAsync("http://localhost:51120/api/test/path", content);
                string response = await responseMessage.Content.ReadAsStringAsync();
                List<Point> points = JsonConvert.DeserializeObject<List<Point>>(response);

                Point startPoint = points.First();
                Point currentPoint = startPoint.OutgoingConnection.To;
                items.Add(itemIdMapping[currentPoint.Id]);
                float cost = startPoint.OutgoingConnection.Cost;
                float lastCost = 0;
                while (currentPoint != startPoint && currentPoint.OutgoingConnection.To != null)
                {
                    cost += currentPoint.OutgoingConnection.Cost;
                    lastCost = currentPoint.OutgoingConnection.Cost;
                    currentPoint = currentPoint.OutgoingConnection.To;
                    items.Add(itemIdMapping[currentPoint.Id]);
                }
                ViewData["PathCost"] = cost - lastCost;
                /*foreach (Point point in points)
                {
                    if (!point.Id.Equals(0))
                    {
                        items.Add(itemIdMapping[point.Id]);
                    }
                }*/
            }

            return View(items);
        }
    }
}
