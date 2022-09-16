using AutoMapper;
using Mango.Services.ProductsAPI.Contexts;
using Mango.Contracts.Dtos;
using Mango.Contracts.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductsAPI.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDBContext _db;

        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateOrUpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (product.ProductId > 0)
            {
                _db.Products.Update(product);
            } else
            {
                _db.Products.Add(product);
            }
            await _db.SaveChangesAsync();
            productDto.ProductId = product.ProductId;
            return productDto;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            bool IsDeleted = false;
            try
            {
                Product? product = await FindProductByIdAsync(productId);
                if (product != null)
                {
                    _db.Products.Remove(product);
                    await _db.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception)
            {
            }
            return IsDeleted;
        }

        public async Task<ProductDto?> GetProductById(int productId)
        {
            Product? product = await FindProductByIdAsync(productId);
            return _mapper.Map<ProductDto?>(product);
        }

        private async Task<Product?> FindProductByIdAsync(int productId)
        {
            return await _db.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>?> GetProducts()
        {
            IEnumerable<Product>? productList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
