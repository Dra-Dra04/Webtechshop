using Microsoft.EntityFrameworkCore;
using Webtechshop.Models;

namespace Webtechshop.Repository
{
    public class SeedData
    {
            public static void SeedingData(DataContext _context)
            {
                _context.Database.Migrate();
                if (!_context.Products.Any())
                {
                    CategoryModel Phone = new CategoryModel { Name = "Iphone 16 pro", Slug="Phone", Description="Phone is popular of the world", Status="1" };
                    CategoryModel Laptop = new CategoryModel { Name = "Laptop", Slug="Laptop", Description="Laptop is popular of the world", Status="1" };

                    BrandModel Asus = new BrandModel { Name = "Asus", Slug="asus", Description="Asus is large brand in the world", Status="1" };
                    BrandModel Apple = new BrandModel { Name = "Apple", Slug="Apple", Description="Apple is large brand in the world", Status="1" };

                    _context.Products.AddRange(
                        new ProductModel { Name="Tuf F15 Gaming", Slug="Laptop", Description="F15 is the best", Image="tuf.jpg", Category=Laptop, Brand=Asus, CategoryId= 1, BrandId=1, Price= 18500000 },
                        new ProductModel { Name="Iphone 16", Slug="Phone", Description="Ip16 is the best", Image="ip16.jpg", Category=Phone, Brand=Apple, CategoryId= 2, BrandId=1, Price= 18500000 }
                     );
                    _context.SaveChanges();
                }
            }
    }
}
