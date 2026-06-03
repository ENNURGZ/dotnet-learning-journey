using Business.Models;

namespace Business.Interfaces;

public interface IProductService : IModelService<ProductModel>
{
    Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch);

    Task<IEnumerable<CategoryModel>> GetAllProductCategoriesAsync();

    Task AddCategoryAsync(CategoryModel categoryModel);

    Task UpdateCategoryAsync(CategoryModel categoryModel);

    Task RemoveCategoryAsync(int categoryId);
}
