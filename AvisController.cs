using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoManager.Models.RestosModel;

namespace RestoManager.Controllers
{
    public class AvisController : Controller
    {
        private readonly RestosDbContext _context;

        public AvisController(RestosDbContext context)
        {
            _context = context;
        }

        // GET: Avis
        public async Task<IActionResult> Index()
        {
            var avis = await _context.Avis
                .Include(a => a.LeResto)
                .ToListAsync();
            return View(avis);
        }

        // GET: Avis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var avis = await _context.Avis
                .Include(a => a.LeResto)
                .FirstOrDefaultAsync(m => m.CodeAvis == id);

            if (avis == null) return NotFound();

            return View(avis);
        }

        // GET: Avis/Create
        public IActionResult Create()
        {
            ViewBag.Restaurants = _context.Restaurants.ToList();
            return View();
        }

        // POST: Avis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodeAvis,NomPersonne,Note,Commentaire,NumResto")] Avis avis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Restaurants = _context.Restaurants.ToList();
            return View(avis);
        }

        // GET: Avis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var avis = await _context.Avis.FindAsync(id);
            if (avis == null) return NotFound();

            ViewBag.Restaurants = _context.Restaurants.ToList();
            return View(avis);
        }

        // POST: Avis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeAvis,NomPersonne,Note,Commentaire,NumResto")] Avis avis)
        {
            if (id != avis.CodeAvis) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Avis.Any(e => e.CodeAvis == avis.CodeAvis))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Restaurants = _context.Restaurants.ToList();
            return View(avis);
        }

        // GET: Avis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var avis = await _context.Avis
                .Include(a => a.LeResto)
                .FirstOrDefaultAsync(m => m.CodeAvis == id);

            if (avis == null) return NotFound();

            return View(avis);
        }

        // POST: Avis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avis = await _context.Avis.FindAsync(id);
            if (avis != null) _context.Avis.Remove(avis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Avis/AvisParCode/5
        // Jointure LINQ : avis pour un restaurant par son code
        public async Task<IActionResult> AvisParCode(int? code)
        {
            if (code == null) return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.LesAvis)
                .FirstOrDefaultAsync(r => r.CodeResto == code);

            if (restaurant == null) return NotFound();

            return View(restaurant);
        }
    }
}
