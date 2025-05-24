using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webtechshop.Models;
using Webtechshop.Repository;

namespace Webtechshop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        //sắp xếp theo danh mục Category
        public async Task<IActionResult> Index(String Slug = "")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var productsByCategories = _dataContext.Products.Where(p => p.CategoryId == category.Id);

            return View(await productsByCategories.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
