using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webtechshop.Models;
using Webtechshop.Repository;

namespace Webtechshop.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        //sắp xếp theo danh mục Brand
        public async Task<IActionResult> Index(string Slug = "")
        {
            BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
            if (brand == null)
            {
                return RedirectToAction("Index");
            }
            var productsByBrand = _dataContext.Products.Where(p => p.BrandId == brand.Id);

            return View(await productsByBrand.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
