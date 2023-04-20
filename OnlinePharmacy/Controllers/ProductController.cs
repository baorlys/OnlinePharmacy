using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using OnlinePharmacy.Models;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using System.Web.Helpers;

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
        [Route("{metaCat}")]
        public IActionResult Index(string metaCat)
        {
            dynamic model = new ExpandoObject();

            model.Products = GetProductsByCat(metaCat);
            model.Cat = GetCatByMeta(metaCat);
            model.ChildCategory = GetChildCategory();
            ViewBag.catName = GetCatByMeta(metaCat).Name;
            return View(model);
        }
        [HttpGet]
        [Route("{metaCat}/{metaProduct}")]
        public IActionResult ProductDetail(string metaProduct)
        {
            dynamic model = new ExpandoObject();
            ViewBag.productName = GetProductDetail(metaProduct).Product.Name;
            model.ProductDetail = GetProductDetail(metaProduct);
            return View(model);
        }






        public const string CARTKEY = "c";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            string jsoncart = JsonConvert.SerializeObject(ls,settings);
            session.SetString(CARTKEY, jsoncart);
        }

        // Hiện thị giỏ hàng
        [Route("/gio-hang", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }


        [Route("addcart/{productId:int}")]
        public IActionResult AddToCart([FromRoute] int productId)
        {

            var product = _db.Products
                            .Where(p => p.Id == productId)
                            .FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productId);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product});
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("addcart/{productId:int}/{quantity:int}", Name ="addcartwithquantity")]
        public IActionResult AddToCart([FromRoute] int productId, [FromRoute] int quantity)
        {

            var product = _db.Products
                            .Where(p => p.Id == productId)
                            .FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");
            return RedirectToAction(nameof(Cart));
        }

        /// xóa item trong cart
        [Route("/removecart/{productId:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productId)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productId);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productId, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productId);
            if(quantity == 0)
            {
                cart.Remove(cartitem);
            }
            else if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }
        [HttpGet]
        [Route("thanh-toan")]
        public IActionResult Checkout()
        {
            ViewBag.Cart = GetCartItems();
            return View();
        }

        [HttpPost]
        [Route("thanh-toan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("Id,Name,Email,Phone,Address,CreateAt,ModifiedAt,DeletedAt")] Customer customer, string paymentMethod)

        {
            if (ModelState.IsValid)
            {
                int cusId;
                customer.CreateAt = DateTime.Now;
                if(CustomerIsExist(customer.Phone))
                {
                    var cusData = GetCustomerByPhone(customer.Phone);
                    cusId = cusData.Id;
                }
                else
                {
                    _db.Add(customer);
                    await _db.SaveChangesAsync();
                    cusId = GetCustomerByPhone(customer.Phone).Id;
                }
                var cart = GetCartItems();
                var totalPrice = 0;
                foreach (var item in cart)
                {
                    totalPrice += Int32.Parse(item.product.Price) * item.quantity;
                }
                var order = new Order
                {
                    CustomerId = cusId,
                    ShippingAddress = customer.Address,
                    ShippingFee = 0,
                    TotalPrice = totalPrice,
                    PaymentMethod = paymentMethod,
                    CreateAt = DateTime.Now
                };
                _db.Add(order);
                await _db.SaveChangesAsync();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.product.Id,
                        Quantity = item.quantity,
                        Price = item.product.Price,
                        CreateAt = DateTime.Now
                    };
                    var product = GetProductById(item.product.Id);
                    product.Inventory = product.Inventory - item.quantity;
                    _db.Update(product);
                    _db.Add(orderDetail);
                }
                await _db.SaveChangesAsync();
                ClearCart();
                return RedirectToAction("Index","Home");
            }
            return View(GetCartItems());
        }

        public bool CustomerIsExist(string phone) { 
            return _db.Customers.Any(x => x.Phone == phone);
        }

        public Customer GetCustomerByPhone(string phone)
        {
            return _db.Customers.FirstOrDefault(x => x.Phone == phone);
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

        public ProductDetail GetProductDetail(string metaProduct)
        {
            var product = _db.Products.Single(x => x.Meta == metaProduct);
            var category = from pc in _db.ProductCategories
                           where pc.Id == product.CategoryId
                           select pc;
            return new ProductDetail()
            {
                Product = product,
                Category = category.FirstOrDefault(),
            };
        }

        public Product GetProductById(int id)
        {
            var product = _db.Products.Single(x => x.Id == id);
            return product;
        }



    }
}
