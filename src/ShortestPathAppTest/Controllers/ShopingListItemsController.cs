using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickShopper.Data;
using QuickShopper.Models;

namespace QuickShopper.Controllers
{
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
    }
}
