using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
           return new List<Product>()
           {
               new Product()
               {
                   Name = "Iphone X",
                   Description = "asd",
                   Summary = "asd",
                   Category = "Phones",
                   Price = 950M,
                   ImageFile = "image1.png"


               }
           };
        }
    }
}
