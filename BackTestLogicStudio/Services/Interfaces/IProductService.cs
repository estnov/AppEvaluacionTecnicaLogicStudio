using BackTestLogicStudio.Models;
using BackTestLogicStudio.Models.Dtos;

namespace BackTestLogicStudio.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<ProductDto?> GetById(int id);
        Task<bool> Create(ProductDto dto);
        Task<bool> Delete(int id);
        Task<ProductDto?> Update(int id, ProductUpdateDto dto);
    }
}
