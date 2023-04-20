using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlinePharmacy.Controllers
{
    public class SearchController : Controller
    {
        OnlinePharmacyContext _context = new OnlinePharmacyContext();

        [HttpGet]
        [Route("tim-kiem")]
        public IActionResult Result(string query)
        {
            var queryMeta = SupFunc.ConvertToMeta(query);
            var productResults = _context.Products.Where(p => p.Meta.Contains(queryMeta));
            var blogResults = _context.Blogs.Where(b => b.Meta.Contains(queryMeta));
            var categoryResults = _context.ProductCategories;

            var viewModel = new SearchResultsViewModel
            {
                Query = query,
                ProductResults = productResults.ToList(),
                CategoryResults = categoryResults.ToList(),
                BlogResults = blogResults.ToList()
            };

            return View(viewModel);
        }
    }
}
