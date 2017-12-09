using ECommerce.ProductCatalog.Domain;

namespace ECommerce.CheckoutService.Domain
{
    public class CheckoutProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}