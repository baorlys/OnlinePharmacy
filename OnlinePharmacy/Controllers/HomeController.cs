using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Models;
using System.Diagnostics;
using System.Dynamic;

namespace OnlinePharmacy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OnlinePharmacyContext _db = new OnlinePharmacyContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.Products = GetProducts();
            model.ProductCategory = GetProductCategory();
            model.ChildCategory = GetChildCategory();
            model.Blogs = GetBlogs();
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Product> GetProducts()
        {
            var products = from p in _db.Products
                           select p;
            return products.ToList();
        }


        public List<ProductCategory> GetProductCategory()
        {
            var categories = from pc in _db.ProductCategories
                           select pc;
            return categories.ToList();
        }

        public List<ProductCategory> GetChildCategory()
        {
            var categories = from pc in _db.ProductCategories
                             where pc.ParentId != 0
                             select pc;
            return categories.ToList();
        }

        public List<Blog> GetBlogs()
        {
            var blogs = _db.Blogs
                     .OrderByDescending(x => x.Id)
                     .Take(6)
                     .ToList();
            return blogs;
        }


    }
}