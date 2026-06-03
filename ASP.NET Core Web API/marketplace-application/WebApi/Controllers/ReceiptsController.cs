using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.Models;
using Business.Validation;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        this.receiptService = receiptService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAll()
    {
        var receipts = await this.receiptService.GetAllAsync();
        return Ok(receipts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReceiptModel>> GetById(int id)
    {
        try
        {
            var receipt = await this.receiptService.GetByIdAsync(id);
            return Ok(receipt);
        }
        catch (MarketException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(ex.Message);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ReceiptModel>> Create([FromBody] ReceiptModel receipt)
    {
        try
        {
            var result = await this.receiptService.AddAsync(receipt);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ReceiptModel receipt)
    {
        if (receipt == null || receipt.Id != id)
        {
            return BadRequest("Receipt ID mismatch.");
        }

        try
        {
            await this.receiptService.UpdateAsync(receipt);
            return NoContent();
        }
        catch (MarketException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
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
            await this.receiptService.DeleteAsync(id);
            return NoContent();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetDetails(int id)
    {
        try
        {
            var details = await this.receiptService.GetReceiptDetailsAsync(id);
            return Ok(details);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/total")]
    public async Task<ActionResult<decimal>> GetTotal(int id)
    {
        try
        {
            var total = await this.receiptService.ToPayAsync(id);
            return Ok(total);
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/checkout")]
    public async Task<ActionResult> Checkout(int id)
    {
        try
        {
            await this.receiptService.CheckOutAsync(id);
            return Ok();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{receiptId}/products/{productId}")]
    public async Task<ActionResult> AddProduct(int receiptId, int productId, [FromQuery] int quantity)
    {
        try
        {
            await this.receiptService.AddProductAsync(productId, receiptId, quantity);
            return Ok();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{receiptId}/products/{productId}")]
    public async Task<ActionResult> RemoveProduct(int receiptId, int productId, [FromQuery] int quantity)
    {
        try
        {
            await this.receiptService.RemoveProductAsync(productId, receiptId, quantity);
            return Ok();
        }
        catch (MarketException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("period")]
    public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptsByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var receipts = await this.receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
        return Ok(receipts);
    }
}