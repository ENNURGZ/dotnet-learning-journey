using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ReceiptService : IReceiptService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        return this.mapper.Map<IEnumerable<ReceiptModel>>(receipts);
    }

    public async Task<ReceiptModel> GetByIdAsync(int id)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {id} not found.");
        }

        return this.mapper.Map<ReceiptModel>(receipt);
    }

    public async Task<ReceiptModel> AddAsync(ReceiptModel model)
    {
        var receipt = this.mapper.Map<Receipt>(model);
        await this.unitOfWork.ReceiptRepository.AddAsync(receipt);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<ReceiptModel>(receipt);
    }

    public async Task UpdateAsync(ReceiptModel model)
    {
        var receipt = this.mapper.Map<Receipt>(model);
        this.unitOfWork.ReceiptRepository.Update(receipt);
        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
        if (receipt != null)
        {
            if (receipt.ReceiptDetails != null)
            {
                foreach (var detail in receipt.ReceiptDetails.ToList())
                {
                    this.unitOfWork.ReceiptDetailRepository.Delete(detail);
                }
            }

            await this.unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
            await this.unitOfWork.SaveAsync();
        }
    }

    public async Task AddProductAsync(int productId, int receiptId, int quantity)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        var existingDetail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);
        if (existingDetail != null)
        {
            existingDetail.Quantity += quantity;
        }
        else
        {
            var product = await this.unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new MarketException($"Product with id {productId} not found.");
            }

            var customer = receipt.Customer;
            if (customer == null)
            {
                customer = await this.unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(receipt.CustomerId);
            }

            var discountValue = customer?.DiscountValue ?? 0;
            decimal discountUnitPrice = product.Price - (product.Price * discountValue / 100.0m);

            var newDetail = new ReceiptDetail
            {
                ReceiptId = receiptId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price,
                DiscountUnitPrice = discountUnitPrice,
            };

            await this.unitOfWork.ReceiptDetailRepository.AddAsync(newDetail);
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        var existingDetail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);
        if (existingDetail != null)
        {
            if (existingDetail.Quantity <= quantity)
            {
                this.unitOfWork.ReceiptDetailRepository.Delete(existingDetail);
            }
            else
            {
                existingDetail.Quantity -= quantity;
            }

            await this.unitOfWork.SaveAsync();
        }
    }

    public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        return this.mapper.Map<IEnumerable<ReceiptDetailModel>>(receipt.ReceiptDetails);
    }

    public async Task<decimal> ToPayAsync(int receiptId)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        return receipt.ReceiptDetails?.Sum(rd => rd.DiscountUnitPrice * rd.Quantity) ?? 0;
    }

    public async Task CheckOutAsync(int receiptId)
    {
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
        if (receipt == null)
        {
            throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        receipt.IsCheckedOut = true;
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var matchingReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
        return this.mapper.Map<IEnumerable<ReceiptModel>>(matchingReceipts);
    }
}
