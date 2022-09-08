using AutoMapper;
using Mango.Services.ProductsAPI.Contexts;
using Mango.Services.ProductsAPI.DTOs;
using Mango.Services.ProductsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<ProductDTO> CreateOrUpdateProduct(ProductDTO productDTO)
        {
            Product product = _mapper.Map<Product>(productDTO);

            if (product.ProductId > 0)
            {
                _db.Products.Update(product);
            } else
            {
                _db.Products.Add(product);
            }
            await _db.SaveChangesAsync();
            productDTO.ProductId = product.ProductId;
            return productDTO;
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

        public async Task<ProductDTO?> GetProductById(int productId)
        {
            Product? product = await FindProductByIdAsync(productId);
            return _mapper.Map<ProductDTO?>(product);
        }

        private async Task<Product?> FindProductByIdAsync(int productId)
        {
            return await _db.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDTO>?> GetProducts()
        {
            IEnumerable<Product>? productList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(productList);
        }
    }
}
