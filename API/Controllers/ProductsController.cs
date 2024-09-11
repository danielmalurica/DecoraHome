using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastucture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> productRepository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? category, string? sort)
    {
        var spec = new ProductSpecification(brand, category, sort);
        var products = await productRepository.ListAsync(spec);
        //return Ok(await productRepository.GetProductsAsync(brand, category, sort));
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await productRepository.ListAsync(spec));
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        var spec = new CategoryListSpecification();
        return Ok(await productRepository.ListAsync(spec));
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        productRepository.Add(product);
        if (await productRepository.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Cannot update this product");
        }

        productRepository.Update(product);
        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();

        productRepository.Delete(product);
        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the product");
    }

    private bool ProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}

