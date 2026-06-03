using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.Models;
using Business.Validation;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService customerService;

    public CustomersController(ICustomerService customerService)
    {
        this.customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAll()
    {
        var customers = await this.customerService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerModel>> GetById(int id)
    {
        try
        {
            var customer = await this.customerService.GetByIdAsync(id);
            return Ok(customer);
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
    public async Task<ActionResult<CustomerModel>> Create([FromBody] CustomerModel customer)
    {
        try
        {
            var result = await this.customerService.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] CustomerModel customer)
    {
        if (customer == null || customer.Id != id)
        {
            return BadRequest("Customer ID mismatch.");
        }

        try
        {
            await this.customerService.UpdateAsync(customer);
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
            await this.customerService.DeleteAsync(id);
            return NoContent();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("by-product/{productId}")]
    public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomersByProductId(int productId)
    {
        var customers = await this.customerService.GetCustomersByProductIdAsync(productId);
        return Ok(customers);
    }
}