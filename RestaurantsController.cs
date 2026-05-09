using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoManager.Models.RestosModel;

namespace RestoManager.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly RestosDbContext _context;

        public RestaurantsController(RestosDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.LeProprio)
                .ToListAsync();
            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.LeProprio)
                .FirstOrDefaultAsync(m => m.CodeResto == id);

            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            ViewBag.Proprietaires = _context.Proprietaires.ToList();
            return View();
        }

        // POST: Restaurants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodeResto,NomResto,Specialite,Ville,Tel,NumProp")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Proprietaires = _context.Proprietaires.ToList();
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();

            ViewBag.Proprietaires = _context.Proprietaires.ToList();
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeResto,NomResto,Specialite,Ville,Tel,NumProp")] Restaurant restaurant)
        {
            if (id != restaurant.CodeResto) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Restaurants.Any(e => e.CodeResto == restaurant.CodeResto))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Proprietaires = _context.Proprietaires.ToList();
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.LeProprio)
                .FirstOrDefaultAsync(m => m.CodeResto == id);

            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null) _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Restaurants/VoirProprietaires
        public async Task<IActionResult> VoirProprietaires()
        {
            var proprietaires = await _context.Proprietaires
                .Include(p => p.LesRestos)
                .ToListAsync();
            return View(proprietaires);
        }

        // GET: Restaurants/AvisParRestaurant/5
        // Jointure via propriétés de navigation
        public async Task<IActionResult> AvisParRestaurant(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.LeProprio)
                .Include(r => r.LesAvis)
                .FirstOrDefaultAsync(r => r.CodeResto == id);

            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        // GET: Restaurants/RestaurantsMoyenne
        
        public async Task<IActionResult> RestaurantsMoyenne()
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.LeProprio)
                .Include(r => r.LesAvis)
                .Where(r => r.LesAvis.Any())
                .ToListAsync();

            var result = restaurants
                .Where(r => r.LesAvis.Average(a => a.Note) >= 3.5)
                .ToList();

            return View(result);
        }
    }
}
