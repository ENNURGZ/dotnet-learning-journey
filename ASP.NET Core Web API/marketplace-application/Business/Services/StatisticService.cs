using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
        var topProducts = details
            .GroupBy(d => d.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(d => d.Quantity), Product = g.First().Product })
            .OrderByDescending(x => x.TotalQuantity)
            .Select(x => x.Product)
            .Take(productCount);

        return this.mapper.Map<IEnumerable<ProductModel>>(topProducts);
    }

    public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var customerReceiptDetails = receipts
            .Where(r => r.CustomerId == customerId && r.ReceiptDetails != null)
            .SelectMany(r => r.ReceiptDetails ?? Enumerable.Empty<ReceiptDetail>());

        var topProducts = customerReceiptDetails
            .GroupBy(d => d.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(d => d.Quantity), Product = g.First().Product })
            .OrderByDescending(x => x.TotalQuantity)
            .Select(x => x.Product)
            .Take(productCount);

        return this.mapper.Map<IEnumerable<ProductModel>>(topProducts);
    }

    public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var periodReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

        var customerSums = periodReceipts
            .GroupBy(r => r.CustomerId)
            .Select(g =>
            {
                var customer = g.First().Customer;
                var sum = g.Sum(r => r.ReceiptDetails?.Sum(rd => rd.DiscountUnitPrice * rd.Quantity) ?? 0);
                var name = customer != null && customer.Person != null ? $"{customer.Person.Name} {customer.Person.Surname}" : string.Empty;
                return new CustomerActivityModel
                {
                    CustomerId = g.Key,
                    CustomerName = name,
                    ReceiptSum = sum,
                };
            })
            .OrderByDescending(c => c.ReceiptSum)
            .Take(customerCount)
            .ToList();

        return customerSums;
    }

    public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var periodReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
        var details = periodReceipts.SelectMany(r => r.ReceiptDetails ?? Enumerable.Empty<ReceiptDetail>());

        var totalIncome = details
            .Where(rd => rd.Product != null && rd.Product.ProductCategoryId == categoryId)
            .Sum(rd => rd.DiscountUnitPrice * rd.Quantity);

        return totalIncome;
    }
}
