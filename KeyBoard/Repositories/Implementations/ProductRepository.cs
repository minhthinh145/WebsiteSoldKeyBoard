using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context , IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Guid> AddProductAsync(ProductDTO productDto)
        {
            bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
            if (!categoryExists)
            {
                throw new Exception("Danh mục không hợp lệ!"); // Có thể dùng BadRequest nếu trong Controller
            }

            var newProduct = _mapper.Map<Product>(productDto);
            _context.Products!.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct.Id;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var deleteProduct = _context.Products.SingleOrDefault(kb => kb.Id == id);
            if (deleteProduct != null) 
            {
                _context.Products.Remove(deleteProduct);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var kb = await _context.Products.FindAsync(id);
            return _mapper.Map<ProductDTO>(kb);
        }

        public async Task UpdateProductAsync(Guid id, ProductDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _mapper.Map(productDto, product);
                await _context.SaveChangesAsync();
            }
        }
    }   
}
