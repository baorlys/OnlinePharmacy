using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;

namespace OnlinePharmacy.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private OnlinePharmacyContext _db = new OnlinePharmacyContext();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetMenu();
            return View(items);
        }

        private Task<List<Menu>> GetMenu()  {

            var v = from t in _db.Menus
                    where t.Hide == false
                    select t;
            return v.ToListAsync();
        }
    }
}
