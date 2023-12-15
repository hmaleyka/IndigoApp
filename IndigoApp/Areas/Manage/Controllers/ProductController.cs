using IndigoApp.Areas.Manage.ViewModels;
using IndigoApp.DAL;
using IndigoApp.Helpers;
using IndigoApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndigoApp.Areas.Manage.Controllers
{

    [Area("Manage")]
    public class ProductController : Controller
    {
       private  AppDbContext _dbcontext;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext dbcontext, IWebHostEnvironment env)
        {
            _dbcontext = dbcontext;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Product> products = _dbcontext.product.ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductVM createproductvm)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!createproductvm.ImgUrl.CheckType("image/"))
            {
                ModelState.AddModelError("mainPhoto", "you must only apply the image");
                return View();
            }
            if (!createproductvm.ImgUrl.CheckLong(2097152))
            {
                ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                return View();
            }
            Product product = new Product()
            {
                Name = createproductvm.Name,
                Description = createproductvm.Description,
                ImgUrl = createproductvm.ImgUrl.Upload(_env.WebRootPath, @"\Upload\ProductImage\")
            };

           _dbcontext.product.Add(product);
           _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {

            Product product = _dbcontext.product.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return View();
            }
            UpdateProductVM updateproductvm = new UpdateProductVM()
            {
                Name = product.Name,
                Description = product.Description,
                Image = product.ImgUrl
            };
            return View();
        }

        [HttpPost]
        public IActionResult Update (UpdateProductVM updateproductvm)
        {

            Product existproduct = _dbcontext.product.Where(p=>p.Id== updateproductvm.Id).FirstOrDefault();
            if (existproduct == null)
            {
                return View();
            }

            existproduct.Name = updateproductvm.Name;
            existproduct.Description = updateproductvm.Description;


            if(updateproductvm.ImgUrl != null)
            {

                if (!updateproductvm.ImgUrl.CheckType("image/"))
                {
                    ModelState.AddModelError("mainPhoto", "you must only apply the image");
                    return View();
                }
                if (!updateproductvm.ImgUrl.CheckLong(2097152))
                {
                    ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                    return View();
                }

                var oldphoto = existproduct.ImgUrl.FirstOrDefault();
                

                existproduct.ImgUrl = updateproductvm.ImgUrl.Upload(_env.WebRootPath, @"\Upload\ProductImage\");
            
                _dbcontext.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var product = _dbcontext.product.FirstOrDefault(p=>p.Id== id);

            _dbcontext.Remove(product);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
