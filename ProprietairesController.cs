using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoManager.Models.RestosModel;

namespace RestoManager.Controllers
{
    public class ProprietairesController : Controller
    {
        private readonly RestosDbContext _context;

        public ProprietairesController(RestosDbContext context)
        {
            _context = context;
        }

        // GET: Proprietaires
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proprietaires.ToListAsync());
        }

        // GET: Proprietaires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proprietaire = await _context.Proprietaires
                .FirstOrDefaultAsync(m => m.Numero == id);

            if (proprietaire == null) return NotFound();

            return View(proprietaire);
        }

        // GET: Proprietaires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proprietaires/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Nom,Email,Gsm")] Proprietaire proprietaire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proprietaire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proprietaire = await _context.Proprietaires.FindAsync(id);
            if (proprietaire == null) return NotFound();

            return View(proprietaire);
        }

        // POST: Proprietaires/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Numero,Nom,Email,Gsm")] Proprietaire proprietaire)
        {
            if (id != proprietaire.Numero) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proprietaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Proprietaires.Any(e => e.Numero == proprietaire.Numero))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proprietaire = await _context.Proprietaires
                .FirstOrDefaultAsync(m => m.Numero == id);

            if (proprietaire == null) return NotFound();

            return View(proprietaire);
        }

        // POST: Proprietaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proprietaire = await _context.Proprietaires.FindAsync(id);
            if (proprietaire != null) _context.Proprietaires.Remove(proprietaire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
