using KeyBoard.Data;
using KeyBoard.DTOs;
using AutoMapper;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.DTOs.AuthenDTOs;
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
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<CartItemDTO, Cart>(); 
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.UtcNow)); 

            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<HoaDon, HoaDonDTO>().ReverseMap();
            CreateMap<ChiTietHoaDon, ChiTietHoaDonDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserProfileDTO>().ReverseMap();
        }
    }
}
