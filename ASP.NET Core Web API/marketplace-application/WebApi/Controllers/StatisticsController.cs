using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Business.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticService statisticService;

    public StatisticsController(IStatisticService statisticService)
    {
        this.statisticService = statisticService;
    }

    [HttpGet("most-popular-products")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts([FromQuery] int count)
    {
        if (count < 0)
        {
            return BadRequest("Count cannot be negative.");
        }

        var products = await this.statisticService.GetMostPopularProductsAsync(count);
        return Ok(products);
    }

    [HttpGet("customers-most-popular-products")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts([FromQuery] int count, [FromQuery] int customerId)
    {
        if (count < 0 || customerId < 0)
        {
            return BadRequest("Count and CustomerId cannot be negative.");
        }

        var products = await this.statisticService.GetCustomersMostPopularProductsAsync(count, customerId);
        return Ok(products);
    }

    [HttpGet("most-valuable-customers")]
    public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostValuableCustomers([FromQuery] int count, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("StartDate cannot be after EndDate.");
        }

        var customers = await this.statisticService.GetMostValuableCustomersAsync(count, startDate, endDate);
        return Ok(customers);
    }

    [HttpGet("income-of-category")]
    public async Task<ActionResult<decimal>> GetIncomeOfCategory([FromQuery] int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("StartDate cannot be after EndDate.");
        }

        var income = await this.statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
        return Ok(income);
    }
}