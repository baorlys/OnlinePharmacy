using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Models;
using System.Diagnostics;
using System.Dynamic;

namespace OnlinePharmacy.Controllers
{
    [Route("san-pham")]
    public class ProductController : Controller
    {
        OnlinePharmacyContext _db = new OnlinePharmacyContext();

        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("{meta}")]
        public IActionResult Index(string meta, int? page)
        {
            dynamic model = new ExpandoObject();
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            model.Products = GetProductsByCat(meta);
            model.Cat = GetCatByMeta(meta);
            model.ChildCategory = GetChildCategory();
            ViewBag.catName = GetCatByMeta(meta).Name;
            return View(model.ToPageList(pageNumber,pageSize));
        }
        [HttpGet]
        [Route("{meta}/{name}")]
        public IActionResult ProductDetail(int id)
        {
            dynamic model = new ExpandoObject();
            ViewBag.productName = GetProductDetail(id).Product.Name;
            model.ProductDetail = GetProductDetail(id);
            return View(model);
        }
        public List<ProductCategory> GetChildCategory()
        {
            var categories = from pc in _db.ProductCategories
                             where pc.ParentId != 0
                             select pc;
            return categories.ToList();
        }
        public List<Product> GetProductsByCat(string meta)
        {
            ProductCategory cat = GetCatByMeta(meta);
            List<int> listCatId = new List<int>();
            listCatId.Add(cat.Id);
            if(cat.ParentId == 0)
            {
                var child = from c in _db.ProductCategories
                            where cat.Id == c.ParentId
                            select c;
                foreach(var item in child)
                {
                    listCatId.Add(item.Id);
                }
            }

            var products = from p in _db.Products
                           where listCatId.Contains((int)p.CategoryId)
                           select p;
            return products.ToList();
        }


        public ProductCategory GetCatByMeta(string meta)
        {
            var cat = from pc in _db.ProductCategories
                      where pc.Meta == meta
                      select pc;
            return cat.FirstOrDefault();
        }

        public ProductDetail GetProductDetail(int id)
        {
            var product = from p in _db.Products
                          where p.Id == id
                          select p;
            var category = from pc in _db.ProductCategories
                           where pc.Id == product.FirstOrDefault().CategoryId
                           select pc;
            var inventory = from pi in _db.ProductInventories
                            where pi.Id == product.FirstOrDefault().Id
                            select pi;
            return new ProductDetail()
            {
                Product = product.FirstOrDefault(),
                Category = category.FirstOrDefault(),
                Inventory = inventory.FirstOrDefault()
            };
        }

    }
}
