using KeyBoard.Data;
using KeyBoard.DTOs;
using AutoMapper;
namespace KeyBoard.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<Cart, CartItemDTO>()
          .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<CartItemDTO, Cart>(); // Cái này vẫn được giữ nguyên

        }
    }
}
