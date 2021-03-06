﻿using ECommerce.ProductCatalog.Domain;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    public class ServiceFabricProductRepository : IProductRepository
    {

        private readonly IReliableStateManager _stateManager;

        public ServiceFabricProductRepository(IReliableStateManager stateManager) {
            _stateManager = stateManager;
        }

        public async Task AddProduct(Product product)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using (var tx = _stateManager.CreateTransaction()) {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var results = new List<Product>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumerator.Current;
                        results.Add(current.Value);
                    }
                }

            }

            return results;
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var results = new List<Product>();

            using (var tx = _stateManager.CreateTransaction())
            {
                return await products.GetOrAddAsync(tx, productId, new Product());

            }

        }
    }
}
