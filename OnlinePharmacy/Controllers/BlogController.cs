using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Models;
using System.Dynamic;

namespace OnlinePharmacy.Controllers
{

    public class BlogController : Controller
    {
        OnlinePharmacyContext _db = new OnlinePharmacyContext();
        private readonly ILogger<BlogController> _logger;
        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        [Route("bai-viet")]
        public IActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.Blogs = GetBlogs();
            model.BlogTags = GetBlogTags();
            return View(model);
        }

        public IActionResult BlogSideBar()
        {
            dynamic model = new ExpandoObject();
            model.Blogs = GetBlogs();
            model.BlogTags = GetBlogTags();
            return View(model);
        }


        public IActionResult BlogDetail(int id)
        {
            dynamic model = new ExpandoObject();
            var blog = GetBlogById(id);
            model.Blog = blog;
            model.Blogs = GetBlogs();
            model.BlogTags = GetBlogTagsOfId(blog.Tags);
            return View(model);
        }

        public List<Blog> GetBlogs()
        {
            var blogs = from b in _db.Blogs
                           select b;
            return blogs.ToList();
        }

        public List<BlogTag> GetBlogTags()
        {
            var blogTags = from bt in _db.BlogTags
                           select bt;
            return blogTags.ToList();
        }

        public Blog GetBlogById(int id)
        {
            var blog = from b in _db.Blogs
                       where b.Id == id
                       select b;
            return blog.FirstOrDefault();
        }

        public List<BlogTag> GetBlogTagsOfId(string tags)
        {
            var temp = tags.Split(",").Select(Int32.Parse).ToList(); ;
            var blogTags = from bt in _db.BlogTags
                           where temp.Contains((int)bt.Id)
                           select bt;
            return blogTags.ToList();
        }
    }
}
