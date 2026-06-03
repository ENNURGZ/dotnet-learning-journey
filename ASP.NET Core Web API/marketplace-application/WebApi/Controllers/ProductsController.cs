using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.Models;
using Business.Validation;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
    {
        var products = await this.productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetById(int id)
    {
        try
        {
            var product = await this.productService.GetByIdAsync(id);
            return Ok(product);
        }
        catch (MarketException ex) when (ex.Message.Contains("not found", System.StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(ex.Message);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> Create([FromBody] ProductModel product)
    {
        try
        {
            var result = await this.productService.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ProductModel product)
    {
        if (product == null || product.Id != id)
        {
            return BadRequest("Product ID mismatch.");
        }

        try
        {
            await this.productService.UpdateAsync(product);
            return NoContent();
        }
        catch (MarketException ex) when (ex.Message.Contains("not found", System.StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(ex.Message);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await this.productService.DeleteAsync(id);
            return NoContent();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("filter")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter([FromBody] FilterSearchModel filterSearch)
    {
        var products = await this.productService.GetByFilterAsync(filterSearch);
        return Ok(products);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
    {
        var categories = await this.productService.GetAllProductCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<ActionResult> AddCategory([FromBody] CategoryModel categoryModel)
    {
        try
        {
            await this.productService.AddCategoryAsync(categoryModel);
            return CreatedAtAction(nameof(GetAllCategories), null, categoryModel);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("categories/{id}")]
    public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryModel categoryModel)
    {
        if (categoryModel == null || categoryModel.Id != id)
        {
            return BadRequest("Category ID mismatch.");
        }

        try
        {
            await this.productService.UpdateCategoryAsync(categoryModel);
            return NoContent();
        }
        catch (MarketException ex) when (ex.Message.Contains("not found", System.StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(ex.Message);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> RemoveCategory(int id)
    {
        try
        {
            await this.productService.RemoveCategoryAsync(id);
            return NoContent();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}