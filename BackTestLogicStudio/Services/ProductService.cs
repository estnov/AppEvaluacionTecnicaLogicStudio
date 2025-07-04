using BackTestLogicStudio.Models;
using BackTestLogicStudio.Models.Dtos;
using BackTestLogicStudio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackTestLogicStudio.Services
{
    public class ProductService : IProductService
    {
        private readonly DbtestLogicStudioContext _ctx;

        public ProductService(DbtestLogicStudioContext ctx) => _ctx = ctx;

        public async Task<bool> Create(ProductDto dto)
        {
            var aux = new Producto
            {
                IdCategoria = dto.IdCategoria,
                Descripcion = dto.Descripcion,
                Imagen = dto.Imagen,
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            _ctx.Productos.Add(aux);

            await _ctx.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _ctx.Productos.FindAsync(id);
            
            if (entity is null) 
                return false;

            _ctx.Productos.Remove(entity);
            
            await _ctx.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            List<ProductDto> products = new List<ProductDto>();

            products = await _ctx.Productos
                            .OrderBy(x => x.Id)
                            .Select(p => new ProductDto
                            {
                                Id = p.Id,
                                Descripcion = p.Descripcion,
                                IdCategoria = p.IdCategoria,
                                Imagen = p.Imagen,
                                Nombre = p.Nombre,
                                Precio = p.Precio,
                                Stock = p.Stock
                            }).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            List<CategoryDto> categories = new List<CategoryDto>();

            categories = await _ctx.Categoria
                                .OrderBy (c => c.Id)
                                .Select(c => new CategoryDto
                                {
                                    Id = c.Id,
                                    Descripcion = c.Descripcion
                                }).ToListAsync();

            return categories;
        }

        public async Task<ProductDto?> GetById(int id)
        {
            ProductDto product = new ProductDto();

            product = await _ctx.Productos
                            .Where(p =>  p.Id == id)
                            .Select(p => new ProductDto
                            {
                                Id = p.Id,
                                Descripcion = p.Descripcion,
                                Stock = p.Stock,
                                IdCategoria = p.IdCategoria,
                                Imagen = p.Imagen,
                                Nombre = p.Nombre,
                                Precio = p.Precio
                            }
                            ).FirstOrDefaultAsync();

            return product;
        }

        public async Task<ProductDto?> Update(int id, ProductUpdateDto dto)
        {
            var entity = await _ctx.Productos.FindAsync(id);

            if (entity == null)
                return null;

            if (dto.Nombre is not null) 
                entity.Nombre = dto.Nombre;

            if (dto.Descripcion is not null) 
                entity.Descripcion = dto.Descripcion;

            if (dto.Precio is not null) 
                entity.Precio = dto.Precio.Value;

            if (dto.Stock is not null) 
                entity.Stock = dto.Stock.Value;

            if (dto.IdCategoria is not null) 
                entity.IdCategoria = dto.IdCategoria.Value;

            if (dto.Imagen is not null) 
                entity.Imagen = dto.Imagen;

            await _ctx.SaveChangesAsync();

            return new ProductDto
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion,
                IdCategoria = entity.IdCategoria,
                Imagen = entity.Imagen,
                Nombre = entity.Nombre,
                Precio = entity.Precio,
                Stock = entity.Stock
            };
        }
    }
}
