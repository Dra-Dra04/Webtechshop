using System.ComponentModel.DataAnnotations.Schema;

namespace Webtechshop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
        public int Status { get; set; }
    }
}
