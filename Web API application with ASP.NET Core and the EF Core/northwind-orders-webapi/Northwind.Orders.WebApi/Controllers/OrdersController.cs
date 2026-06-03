using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Orders.WebApi.Models;
using Northwind.Services.Repositories;
using RepositoryOrder = Northwind.Services.Repositories.Order;

namespace Northwind.Orders.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderRepository orderRepository;
    private readonly ILogger<OrdersController> logger;

    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<FullOrder>> GetOrderAsync(long orderId)
    {
        this.logger.LogTrace("Entering GetOrderAsync with orderId {OrderId}.", orderId);

        try
        {
            var order = await this.orderRepository.GetOrderAsync(orderId);
            var fullOrder = MapToFullOrder(order);
            return this.Ok(fullOrder);
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogWarning(ex, "Order with id {OrderId} not found.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred in GetOrderAsync with orderId {OrderId}.", orderId);
            return this.StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefOrder>>> GetOrdersAsync([FromQuery] int? skip, [FromQuery] int? count)
    {
        int skipVal = skip ?? 0;
        int countVal = count ?? 10;

        this.logger.LogTrace("Entering GetOrdersAsync with skip {Skip}, count {Count}.", skipVal, countVal);

        if (skipVal < 0 || countVal <= 0)
        {
            this.logger.LogWarning("Invalid parameters in GetOrdersAsync: skip = {Skip}, count = {Count}.", skipVal, countVal);
            return this.BadRequest();
        }

        try
        {
            var orders = await this.orderRepository.GetOrdersAsync(skipVal, countVal);
            var briefOrders = orders.Select(MapToBriefOrder).ToList();
            return this.Ok(briefOrders);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            this.logger.LogWarning(ex, "Argument out of range in GetOrdersAsync.");
            return this.BadRequest();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred in GetOrdersAsync with skip {Skip}, count {Count}.", skipVal, countVal);
            return this.StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddOrder>> AddOrderAsync([FromBody] BriefOrder order)
    {
        if (order == null)
        {
            return this.BadRequest();
        }

        this.logger.LogTrace("Entering AddOrderAsync.");

        try
        {
            var repoOrder = MapToRepositoryOrder(order, 0);
            var newId = await this.orderRepository.AddOrderAsync(repoOrder);
            return this.Ok(new AddOrder { OrderId = newId });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred in AddOrderAsync.");
            return this.StatusCode(500);
        }
    }

    [HttpDelete("{orderId}")]
    public async Task<ActionResult> RemoveOrderAsync(long orderId)
    {
        this.logger.LogTrace("Entering RemoveOrderAsync with orderId {OrderId}.", orderId);

        try
        {
            await this.orderRepository.RemoveOrderAsync(orderId);
            return this.NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogWarning(ex, "Order with id {OrderId} not found for deletion.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred in RemoveOrderAsync with orderId {OrderId}.", orderId);
            return this.StatusCode(500);
        }
    }

    [HttpPut("{orderId}")]
    public async Task<ActionResult> UpdateOrderAsync(long orderId, [FromBody] BriefOrder order)
    {
        if (order == null)
        {
            return this.BadRequest();
        }

        this.logger.LogTrace("Entering UpdateOrderAsync with orderId {OrderId}.", orderId);

        try
        {
            var repoOrder = MapToRepositoryOrder(order, orderId);
            await this.orderRepository.UpdateOrderAsync(repoOrder);
            return this.NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogWarning(ex, "Order with id {OrderId} not found for update.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occurred in UpdateOrderAsync with orderId {OrderId}.", orderId);
            return this.StatusCode(500);
        }
    }

    private static FullOrder MapToFullOrder(RepositoryOrder order)
    {
        return new FullOrder
        {
            Id = order.Id,
            Customer = new Models.Customer
            {
                Code = order.Customer.Code.Code,
                CompanyName = order.Customer.CompanyName,
            },
            Employee = new Models.Employee
            {
                Id = order.Employee.Id,
                FirstName = order.Employee.FirstName,
                LastName = order.Employee.LastName,
                Country = order.Employee.Country,
            },
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Shipper = new Models.Shipper
            {
                Id = order.Shipper.Id,
                CompanyName = order.Shipper.CompanyName,
            },
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShippingAddress = new Models.ShippingAddress
            {
                Address = order.ShippingAddress.Address,
                City = order.ShippingAddress.City,
                Region = order.ShippingAddress.Region,
                PostalCode = order.ShippingAddress.PostalCode,
                Country = order.ShippingAddress.Country,
            },
            OrderDetails = order.OrderDetails.Select(d => new FullOrderDetail
            {
                ProductId = d.Product.Id,
                ProductName = d.Product.ProductName,
                CategoryId = d.Product.CategoryId,
                CategoryName = d.Product.Category,
                SupplierId = d.Product.SupplierId,
                SupplierCompanyName = d.Product.Supplier,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity,
                Discount = d.Discount,
            }).ToList(),
        };
    }

    private static BriefOrder MapToBriefOrder(RepositoryOrder order)
    {
        return new BriefOrder
        {
            Id = order.Id,
            CustomerId = order.Customer.Code.Code,
            EmployeeId = order.Employee.Id,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            ShipperId = order.Shipper.Id,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShipAddress = order.ShippingAddress.Address,
            ShipCity = order.ShippingAddress.City,
            ShipRegion = order.ShippingAddress.Region,
            ShipPostalCode = order.ShippingAddress.PostalCode,
            ShipCountry = order.ShippingAddress.Country,
            OrderDetails = new List<BriefOrderDetail>(),
        };
    }

    private static RepositoryOrder MapToRepositoryOrder(BriefOrder order, long orderId)
    {
        var repoOrder = new RepositoryOrder(orderId)
        {
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            ShipName = order.ShipName,
            Customer = new Northwind.Services.Repositories.Customer(new CustomerCode(order.CustomerId))
            {
                CompanyName = string.Empty,
            },
            Employee = new Northwind.Services.Repositories.Employee(order.EmployeeId)
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Country = string.Empty,
            },
            Shipper = new Northwind.Services.Repositories.Shipper(order.ShipperId)
            {
                CompanyName = string.Empty,
            },
            ShippingAddress = new Northwind.Services.Repositories.ShippingAddress(
                order.ShipAddress,
                order.ShipCity,
                order.ShipRegion,
                order.ShipPostalCode,
                order.ShipCountry),
        };

        if (order.OrderDetails != null)
        {
            foreach (var d in order.OrderDetails)
            {
                repoOrder.OrderDetails.Add(new Northwind.Services.Repositories.OrderDetail(repoOrder)
                {
                    Product = new Northwind.Services.Repositories.Product(d.ProductId)
                    {
                        ProductName = string.Empty,
                        Supplier = string.Empty,
                        Category = string.Empty,
                    },
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount,
                });
            }
        }

        return repoOrder;
    }
}
