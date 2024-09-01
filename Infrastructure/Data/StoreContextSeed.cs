using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastucture.Data;

public class StoreContextSeed
{
    public async static Task SeedAsync(StoreContext storeContext)
    {
        if (!storeContext.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/Products.JSON");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products == null) return;
            storeContext.Products.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
    }
}
