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

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var customers = await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        return this.mapper.Map<IEnumerable<CustomerModel>>(customers);
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
        var customer = await this.unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
        if (customer == null)
        {
            throw new MarketException($"Customer with id {id} not found.");
        }

        return this.mapper.Map<CustomerModel>(customer);
    }

    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        ValidateCustomer(model);

        var customer = this.mapper.Map<Customer>(model);
        await this.unitOfWork.CustomerRepository.AddAsync(customer);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<CustomerModel>(customer);
    }

    public async Task UpdateAsync(CustomerModel model)
    {
        ValidateCustomer(model);

        var customer = this.mapper.Map<Customer>(model);
        this.unitOfWork.CustomerRepository.Update(customer);
        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
    {
        var customers = await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        var matchingCustomers = customers.Where(c => c.Receipts != null && c.Receipts.Any(r => r.ReceiptDetails != null && r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
        return this.mapper.Map<IEnumerable<CustomerModel>>(matchingCustomers);
    }

    private static void ValidateCustomer(CustomerModel? model)
    {
        if (model == null)
        {
            throw new MarketException("Customer model cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new MarketException("Customer Name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(model.Surname))
        {
            throw new MarketException("Customer Surname cannot be empty.");
        }

        if (model.BirthDate < new DateTime(1900, 1, 1) || model.BirthDate > DateTime.Now)
        {
            throw new MarketException("Customer BirthDate is invalid.");
        }

        if (model.DiscountValue < 0)
        {
            throw new MarketException("Customer DiscountValue cannot be negative.");
        }
    }
}
