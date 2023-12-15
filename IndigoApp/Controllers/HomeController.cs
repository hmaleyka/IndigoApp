using IndigoApp.DAL;
using IndigoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndigoApp.Controllers
{
    public class HomeController : Controller
    {

        private AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                products=await _db.product.ToListAsync(),
            };
            return View(homeVM);
        }
    }
}
