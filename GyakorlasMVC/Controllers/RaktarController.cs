using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GyakorlasMVC.Data;
using GyakorlasMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace GyakorlasMVC.Controllers
{
    public class RaktarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaktarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Adatmodels
        public async Task<IActionResult> Index(string megnevezesKereses, string tipusKereses)
        {
            Arukereso keresoAru = new Arukereso();
            var Aru = _context.Adatmodel.Select(x => x);
            if (!string.IsNullOrEmpty(megnevezesKereses))
            {
                keresoAru.megnevezesKereses = megnevezesKereses;
                Aru = Aru.Where(x => x.Megnevezes.Contains(megnevezesKereses));
            }

            if (!string.IsNullOrEmpty(tipusKereses))
            {
                keresoAru.tipusKereses = tipusKereses;
                Aru = Aru.Where(x => x.Tipus.Equals(tipusKereses));
            }

            keresoAru.tipusLista = new SelectList(await _context.Adatmodel.Select(x => x.Tipus).Distinct().OrderBy(x => x).ToListAsync());
            keresoAru.Aru = await Aru.ToListAsync();

            return View(keresoAru);
        }

        // GET: Raktar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adatmodel = await _context.Adatmodel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adatmodel == null)
            {
                return NotFound();
            }

            return View(adatmodel);
        }

        // GET: Raktar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Raktar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Megnevezes,Gyarto,Tipus,BeszerzesiAr")] Adatmodel adatmodel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adatmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adatmodel);
        }

        // GET: Raktar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adatmodel = await _context.Adatmodel.FindAsync(id);
            if (adatmodel == null)
            {
                return NotFound();
            }
            return View(adatmodel);
        }

        // POST: Raktar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Megnevezes,Gyarto,Tipus,BeszerzesiAr")] Adatmodel adatmodel)
        {
            if (id != adatmodel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adatmodel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdatmodelExists(adatmodel.Id))
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
            return View(adatmodel);
        }

        // GET: Raktar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adatmodel = await _context.Adatmodel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adatmodel == null)
            {
                return NotFound();
            }

            return View(adatmodel);
        }

        // POST: Raktar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adatmodel = await _context.Adatmodel.FindAsync(id);
            _context.Adatmodel.Remove(adatmodel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdatmodelExists(int id)
        {
            return _context.Adatmodel.Any(e => e.Id == id);
        }
    }
}
