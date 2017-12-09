using System;
using System.Collections.Generic;

namespace ECommerce.CheckoutService.Domain
{
    public class CheckoutSummary
    {
        public CheckoutSummary() {
            Date = DateTime.Now;
            Products = new List<CheckoutProduct>();
        }

        public List<CheckoutProduct> Products { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
