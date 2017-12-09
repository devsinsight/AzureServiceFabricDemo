using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog.Domain
{
    public interface IProductCatalogService : IService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task AddProduct(Product product);
    }
}
