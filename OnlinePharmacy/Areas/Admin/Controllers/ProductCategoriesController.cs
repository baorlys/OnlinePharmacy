using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;

namespace OnlinePharmacy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]

    public class ProductCategoriesController : AdminBaseController
    {
        private readonly OnlinePharmacyContext _context;

        public ProductCategoriesController(OnlinePharmacyContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index()
        {
              return _context.ProductCategories != null ? 
                          View(await _context.ProductCategories.ToListAsync()) :
                          Problem("Entity set 'OnlinePharmacyContext.ProductCategories'  is null.");
        }

        // GET: Admin/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Create
        public IActionResult Create()
        {
            var parentCategory = _context.ProductCategories.Where(pc => pc.ParentId == 0).ToList();
            ViewBag.parentCategory = parentCategory;
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentId,Name,Meta,Desc,CreateAt,ModifiedAt,DeletedAt")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                var id = _context.ProductCategories.FirstOrDefault(x => x.Meta == productCategory.Meta).Id;
                var highestOrder = _context.Menus.OrderByDescending(x => x.Order).FirstOrDefault();
                var menu = new Menu
                {
                    Id = id,
                    ParentId = productCategory.ParentId,
                    Type = "Product",
                    Title = productCategory.Name,
                    Description = productCategory.Desc,
                    Meta = productCategory.Meta,
                    Order = highestOrder.Order + 1,
                    Hide = true,
                    CreateAt = productCategory.CreateAt
                };
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var haveChild = _context.ProductCategories.Any(x => x.ParentId == id);
            var parentCategory = _context.ProductCategories.Where(pc => pc.ParentId == 0).ToList();
            ViewBag.parentCategory = parentCategory;
            ViewBag.haveChild = haveChild;
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,Name,Meta,Desc,CreateAt,ModifiedAt,DeletedAt")] ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    var menu = _context.Menus.FirstOrDefault(x => x.Id == productCategory.Id);
                    menu.Title = productCategory.Name;
                    menu.Meta = productCategory.Meta;
                    menu.ModifiedAt = productCategory.ModifiedAt;
                    menu.ParentId = productCategory.ParentId;
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.Id))
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
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            var parentCategory = _context.ProductCategories.Where(pc => pc.ParentId == 0).ToList();
            ViewBag.parentCategory = parentCategory;
            var cannotDel = _context.Products.Any(x => x.CategoryId == id);
            var haveChild = _context.ProductCategories.Any(x => x.ParentId == id);
            ViewBag.cannotDel = cannotDel || haveChild;
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductCategories == null)
            {
                return Problem("Entity set 'OnlinePharmacyContext.ProductCategories'  is null.");
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);
            var menu = await _context.Menus.FindAsync(id);
            if (productCategory != null)
            {
                _context.Menus.Remove(menu);
                _context.ProductCategories.Remove(productCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
          return (_context.ProductCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
