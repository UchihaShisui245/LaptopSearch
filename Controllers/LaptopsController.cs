using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApps.Data;
using WebApps.Entities;
using WebApps.Models;

namespace WebApps.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaptopsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Laptops
        public async Task<IActionResult> Index()
        {
            var laptopNewList = new LaptapDto();

            var getItems =await GetItems();

            laptopNewList.LaptopDropDown = getItems.Item1;

            laptopNewList.LaptopTable = getItems.Item2;

            return View(laptopNewList);

        }
        [HttpPost]

        public async Task<IActionResult> Index(LaptapDto laptop)
        {
            var laptopNewList = new LaptapDto();

            var getItems = await GetItems();

            laptopNewList.LaptopDropDown = getItems.Item1;

            laptopNewList.LaptopTable = laptop.Name == null ? getItems.Item2 : getItems.Item2.Where(s => s.Brand == laptop.Name);


            return View(laptopNewList);
        }
        public async Task<IActionResult> GetDetails(int id)
        {
            if (_context.Laptop != null)
            {
                return View(await _context.Laptop.ToListAsync());

            }
            return Problem("Entity set 'ApplicationDbContext.Laptop'  is null.");
        }

        // GET: Laptops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Laptop == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptop
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Laptops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,ModelName")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
        }

        // GET: Laptops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Laptop == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,ModelName")] Laptop laptop)
        {
            if (id != laptop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
        }

        // GET: Laptops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Laptop == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptop
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Laptop == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Laptop'  is null.");
            }
            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptop.Remove(laptop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
            return (_context.Laptop?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [NonAction]

        public async Task<Tuple<IEnumerable<SelectListItem>, IEnumerable<Laptop>>> GetItems()
        {
            var getLaptops = await _context.Laptop.ToListAsync();

            var getUpdatedLaptops = getLaptops.GroupBy(s => s.Brand).Select(s => new SelectListItem() { Text = s.Key, Value = s.Key });

            return Tuple.Create<IEnumerable<SelectListItem>, IEnumerable<Laptop>>(getUpdatedLaptops, getLaptops);
        }
    }
}
