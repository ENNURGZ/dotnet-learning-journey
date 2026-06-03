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

public class ProductService : IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        var products = await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();
        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        var product = await this.unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
        if (product == null)
        {
            throw new MarketException($"Product with id {id} not found.");
        }

        return this.mapper.Map<ProductModel>(product);
    }

    public async Task<ProductModel> AddAsync(ProductModel model)
    {
        ValidateProduct(model);

        var product = this.mapper.Map<Product>(model);
        await this.unitOfWork.ProductRepository.AddAsync(product);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<ProductModel>(product);
    }

    public async Task UpdateAsync(ProductModel model)
    {
        ValidateProduct(model);

        var product = this.mapper.Map<Product>(model);
        this.unitOfWork.ProductRepository.Update(product);
        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
    {
        var products = await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();

        if (filterSearch != null)
        {
            if (filterSearch.CategoryId.HasValue)
            {
                products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId.Value);
            }

            if (filterSearch.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= filterSearch.MinPrice.Value);
            }

            if (filterSearch.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= filterSearch.MaxPrice.Value);
            }
        }

        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }

    public async Task<IEnumerable<CategoryModel>> GetAllProductCategoriesAsync()
    {
        var categories = await this.unitOfWork.CategoryRepository.GetAllAsync();
        return this.mapper.Map<IEnumerable<CategoryModel>>(categories);
    }

    public async Task AddCategoryAsync(CategoryModel categoryModel)
    {
        ValidateCategory(categoryModel);

        var category = this.mapper.Map<ProductCategory>(categoryModel);
        await this.unitOfWork.CategoryRepository.AddAsync(category);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateCategoryAsync(CategoryModel categoryModel)
    {
        ValidateCategory(categoryModel);

        var category = this.mapper.Map<ProductCategory>(categoryModel);
        this.unitOfWork.CategoryRepository.Update(category);
        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveCategoryAsync(int categoryId)
    {
        await this.unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);
        await this.unitOfWork.SaveAsync();
    }

    private static void ValidateProduct(ProductModel? model)
    {
        if (model == null)
        {
            throw new MarketException("Product model cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(model.ProductName))
        {
            throw new MarketException("Product Name cannot be empty.");
        }

        if (model.Price < 0)
        {
            throw new MarketException("Product Price cannot be negative.");
        }
    }

    private static void ValidateCategory(CategoryModel? model)
    {
        if (model == null)
        {
            throw new MarketException("Category model cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new MarketException("Category Name cannot be empty.");
        }
    }
}
