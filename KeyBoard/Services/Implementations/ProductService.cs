using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo , IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<ProductDTO> AddProductAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            if (product == null)
            {
                return null; // Nếu mapping thất bại, trả về null
            }

            var createdProduct = await _repo.AddProductAsync(product);
            return _mapper.Map<ProductDTO>(createdProduct); // Chuyển về DTO để trả về cho client
        }


        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var productToDelete = await _repo.GetProductByIdAsync(id);
            if (productToDelete == null)
            {
                return false; // Không tìm thấy sản phẩm, trả về false
            }

            await _repo.DeleteProductAsync(productToDelete);
            return true; // Xóa thành công, trả về true
        }



        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var productModels = await _repo.GetAllProductsAsync();
            if(productModels != null)
            {
                return _mapper.Map<List<ProductDTO>>(productModels);
            }
            else
            {
                return new List<ProductDTO>();
            }
        }

        public async Task<ProductDTO?> GetProductByIdAsync(Guid id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            
            return product == null ? null : _mapper.Map<ProductDTO>(product);
        }


        public async Task<bool> UpdateProductAsync(Guid id, ProductDTO productDTO)
        {
            var productToUpdate = await _repo.GetProductByIdAsync(id);
            if (productToUpdate == null)
            {
                return false; // Sản phẩm không tồn tại
            }

            // Cập nhật thông tin từ DTO vào thực thể Product
            _mapper.Map(productDTO, productToUpdate);

            await _repo.UpdateProductAsync(productToUpdate);
            return true; // Cập nhật thành công
        }

    }
}
