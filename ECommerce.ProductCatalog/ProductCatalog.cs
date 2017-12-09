using ECommerce.ProductCatalog.Domain;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class ProductCatalog : StatefulService, IProductCatalogService
    {
        private IProductRepository _repository;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        { }

        protected override async Task RunAsync(CancellationToken cancellationToken) {
            _repository = new ServiceFabricProductRepository(this.StateManager);

            var product1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Dell Monitor",
                Description = "Computer Monitor",
                Price = 500,
                Availability = 100
            };
            var product2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Arc Touch Mouse",
                Description = "Computer Mouse, bluetooth, requires 2 AAA batteries",
                Price = 60,
                Availability = 30
            };
            var product3 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Surface Book",
                Description = "Microsoft's Latest Laptop, i7 CPU, 1TB SSD",
                Price = 2200,
                Availability = 15
            };

            await _repository.AddProduct(product1);
            await _repository.AddProduct(product2);
            await _repository.AddProduct(product3);

            IEnumerable<Product> all = await _repository.GetAllProducts();
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[]
            {
                new ServiceReplicaListener(serviceContext => this.CreateServiceRemotingListener(serviceContext))
            };
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _repository.GetAllProducts();
        }

        public async Task AddProduct(Product product)
        {
            await _repository.AddProduct(product);
        }
    }
}
