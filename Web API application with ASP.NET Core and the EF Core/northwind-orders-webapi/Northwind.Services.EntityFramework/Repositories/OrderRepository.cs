using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFramework.Entities;
using Northwind.Services.Repositories;
using RepositoryOrder = Northwind.Services.Repositories.Order;

namespace Northwind.Services.EntityFramework.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext context;

    public OrderRepository(NorthwindContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<IList<RepositoryOrder>> GetOrdersAsync(int skip, int count)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "skip cannot be less than 0.");
        }

        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "count must be greater than 0.");
        }

        return this.GetOrdersInternalAsync(skip, count);
    }

    public async Task<RepositoryOrder> GetOrderAsync(long orderId)
    {
        var entity = await this.context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Category)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Supplier)
            .FirstOrDefaultAsync(o => o.Id == orderId)
            ?? throw new OrderNotFoundException($"Order {orderId} not found.");

        var customer = await this.context.Customers.FindAsync(entity.CustomerId);
        var customersDict = new Dictionary<string, Northwind.Services.EntityFramework.Entities.Customer>();
        if (customer != null)
        {
            customersDict[customer.Id] = customer;
        }
        else
        {
            customersDict[entity.CustomerId] = new Northwind.Services.EntityFramework.Entities.Customer { Id = entity.CustomerId, CompanyName = string.Empty };
        }

        return MapToRepositoryOrder(entity, customersDict);
    }

    public async Task<long> AddOrderAsync(RepositoryOrder order)
    {
        ValidateOrder(order);

        var entity = MapToEntity(order);
        try
        {
            this.context.Orders.Add(entity);
            await this.context.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to add order due to database error.", ex);
        }
    }

    public async Task RemoveOrderAsync(long orderId)
    {
        var entity = await this.context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId)
            ?? throw new OrderNotFoundException($"Order {orderId} not found.");

        this.context.OrderDetails.RemoveRange(entity.OrderDetails);
        this.context.Orders.Remove(entity);

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to remove order due to database error.", ex);
        }
    }

    public async Task UpdateOrderAsync(RepositoryOrder order)
    {
        ValidateOrder(order);

        var existing = await this.context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == order.Id)
            ?? throw new OrderNotFoundException($"Order {order.Id} not found.");

        existing.CustomerId = order.Customer.Code.Code;
        existing.EmployeeId = order.Employee.Id;
        existing.ShipVia = order.Shipper.Id;
        existing.OrderDate = order.OrderDate;
        existing.RequiredDate = order.RequiredDate;
        existing.ShippedDate = order.ShippedDate;
        existing.Freight = order.Freight;
        existing.ShipName = order.ShipName;
        existing.ShipAddress = order.ShippingAddress.Address;
        existing.ShipCity = order.ShippingAddress.City;
        existing.ShipRegion = order.ShippingAddress.Region;
        existing.ShipPostalCode = order.ShippingAddress.PostalCode;
        existing.ShipCountry = order.ShippingAddress.Country;

        this.context.OrderDetails.RemoveRange(existing.OrderDetails);
        existing.OrderDetails.Clear();

        foreach (var detail in order.OrderDetails)
        {
            existing.OrderDetails.Add(new Northwind.Services.EntityFramework.Entities.OrderDetail
            {
                OrderId = existing.Id,
                ProductId = detail.Product.Id,
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount,
            });
        }

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to update order due to database error.", ex);
        }
    }

    private static RepositoryOrder MapToRepositoryOrder(Northwind.Services.EntityFramework.Entities.Order entity, Dictionary<string, Northwind.Services.EntityFramework.Entities.Customer> customers)
    {
        var repoOrder = new RepositoryOrder(entity.Id)
        {
            OrderDate = entity.OrderDate,
            RequiredDate = entity.RequiredDate,
            ShippedDate = entity.ShippedDate,
            Freight = entity.Freight,
            ShipName = entity.ShipName,
        };

        var customerName = customers.TryGetValue(entity.CustomerId, out var c) ? c.CompanyName : string.Empty;
        repoOrder.Customer = new Northwind.Services.Repositories.Customer(new CustomerCode(entity.CustomerId.Trim()))
        {
            CompanyName = customerName,
        };

        repoOrder.Employee = new Northwind.Services.Repositories.Employee(entity.EmployeeId)
        {
            FirstName = entity.Employee?.FirstName ?? string.Empty,
            LastName = entity.Employee?.LastName ?? string.Empty,
            Country = entity.Employee?.Country ?? string.Empty,
        };

        repoOrder.Shipper = new Northwind.Services.Repositories.Shipper(entity.ShipVia)
        {
            CompanyName = entity.Shipper?.CompanyName ?? string.Empty,
        };

        repoOrder.ShippingAddress = new Northwind.Services.Repositories.ShippingAddress(
            entity.ShipAddress,
            entity.ShipCity,
            entity.ShipRegion,
            entity.ShipPostalCode,
            entity.ShipCountry);

        if (entity.OrderDetails != null)
        {
            foreach (var detail in entity.OrderDetails)
            {
                var repoProduct = new Northwind.Services.Repositories.Product(detail.ProductId)
                {
                    ProductName = detail.Product?.ProductName ?? string.Empty,
                    CategoryId = detail.Product?.CategoryId ?? 0,
                    Category = detail.Product?.Category?.CategoryName ?? string.Empty,
                    SupplierId = detail.Product?.SupplierId ?? 0,
                    Supplier = detail.Product?.Supplier?.CompanyName ?? string.Empty,
                };

                var repoDetail = new Northwind.Services.Repositories.OrderDetail(repoOrder)
                {
                    Product = repoProduct,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount,
                };

                repoOrder.OrderDetails.Add(repoDetail);
            }
        }

        return repoOrder;
    }

    private static Northwind.Services.EntityFramework.Entities.Order MapToEntity(RepositoryOrder order)
    {
        var entity = new Northwind.Services.EntityFramework.Entities.Order
        {
            CustomerId = order.Customer.Code.Code,
            EmployeeId = order.Employee.Id,
            ShipVia = order.Shipper.Id,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShipAddress = order.ShippingAddress.Address,
            ShipCity = order.ShippingAddress.City,
            ShipRegion = order.ShippingAddress.Region,
            ShipPostalCode = order.ShippingAddress.PostalCode,
            ShipCountry = order.ShippingAddress.Country,
        };

        foreach (var detail in order.OrderDetails)
        {
            entity.OrderDetails.Add(new Northwind.Services.EntityFramework.Entities.OrderDetail
            {
                ProductId = detail.Product.Id,
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount,
            });
        }

        return entity;
    }

    private static void ValidateOrder(RepositoryOrder order)
    {
        if (order == null)
        {
            throw new RepositoryException("Order cannot be null.");
        }

        if (order.OrderDetails != null)
        {
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Product == null || detail.Product.Id <= 0)
                {
                    throw new RepositoryException("Product ID must be greater than 0.");
                }

                if (detail.UnitPrice < 0)
                {
                    throw new RepositoryException("Unit price cannot be less than 0.");
                }

                if (detail.Quantity <= 0)
                {
                    throw new RepositoryException("Quantity must be greater than 0.");
                }

                if (detail.Discount < 0)
                {
                    throw new RepositoryException("Discount cannot be less than 0.");
                }
            }
        }
    }

    private async Task<IList<RepositoryOrder>> GetOrdersInternalAsync(int skip, int count)
    {
        var entities = await this.context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Category)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Supplier)
            .OrderBy(o => o.Id)
            .Skip(skip)
            .Take(count)
            .ToListAsync();

        var customerIds = entities.Select(o => o.CustomerId).Distinct().ToList();
        var customers = await this.context.Customers
            .Where(c => customerIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var result = new List<RepositoryOrder>();
        foreach (var entity in entities)
        {
            result.Add(MapToRepositoryOrder(entity, customers));
        }

        return result;
    }
}
