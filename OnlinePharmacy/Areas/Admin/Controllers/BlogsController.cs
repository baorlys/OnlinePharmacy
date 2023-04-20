using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;
using ActionNameAttribute = Microsoft.AspNetCore.Mvc.ActionNameAttribute;
using BindAttribute = Microsoft.AspNetCore.Mvc.BindAttribute;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using ValidateAntiForgeryTokenAttribute = Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute;

namespace OnlinePharmacy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class BlogsController : Controller
    {
        private readonly OnlinePharmacyContext _context;

        public BlogsController(OnlinePharmacyContext context)
        {
            _context = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            return _context.Blogs != null ?
                        View(await _context.Blogs.ToListAsync()) :
                        Problem("Entity set 'OnlinePharmacyContext.Blogs'  is null.");
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            dynamic model = new ExpandoObject();
            model.Blog = blog;
            model.BlogTags = GetBlogTagsOfId(blog.Tags);

            return View(model);
        }


        public List<BlogTag> GetBlogTagsOfId(string tags)
        {
            if (string.IsNullOrEmpty(tags)) return null;
            var temp = tags.Split(",").Select(Int32.Parse).ToList();
            var blogTags = from bt in _context.BlogTags
                           where temp.Contains((int)bt.Id)
                           select bt;
            return blogTags.ToList();
        }


        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            var blogTags = _context.BlogTags.ToList();
            ViewBag.TagList = blogTags;
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Content,Thumb,Summary,Tags,CreateAt,ModifiedAt,DeletedAt")] Blog blog, string[] tagArr, IFormFile imageFile)

        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    blog.Thumb = fileName;
                }
                string tagIds = null;
                if (tagArr != null && tagArr.Length > 0)
                {
                    tagIds = string.Join(",", tagArr);
                }
                blog.Tags = tagIds;
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }




        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            var blogTags = _context.BlogTags.ToList();
            ViewBag.Hihi = blogTags;
            ViewBag.PageTags = blogTags;
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Content,Thumb,Summary,Tags,CreateAt,ModifiedAt,DeletedAt")] Blog blog, string[] tagArr, IFormFile imageFile, string thumbName)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }
            var meta = SupFunc.ConvertToMeta(blog.Title);
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    blog.Thumb = fileName;
                }
                string tagIds = null;
                if (tagArr != null && tagArr.Length > 0)
                {
                    tagIds = string.Join(",", tagArr);
                }
                blog.Meta = meta;
                if(blog.Thumb == null) blog.Thumb = thumbName;
                blog.Tags = tagIds;
                _context.Update(blog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(blog.Id))
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

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            dynamic model = new ExpandoObject();
            model.Blog = blog;
            model.BlogTags = GetBlogTagsOfId(blog.Tags);
            return View(model);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'OnlinePharmacyContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
