using System;
using Core.Entities;
using Core.Interfaces;
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
        //return Ok(await productRepository.GetProductsAsync(brand, category, sort));
        return Ok(await productRepository.GetAll());
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
        //return Ok(await productRepository.GetBrandsAsync());
        return Ok();
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        //return Ok(await productRepository.GetCategoriesAsync());
        return Ok();
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

