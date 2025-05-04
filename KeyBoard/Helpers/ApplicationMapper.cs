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
            CreateMap<ApplicationUser, UserProfileDTO>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber)) // Ánh xạ PhoneNumber -> Phone
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
                .ReverseMap()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate));

            // Ánh xạ CreateUserOtpDTO → UserOTP
            CreateMap<CreateUserOtpDTO, UserOTP>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsUsed, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Bỏ qua navigation property

            CreateMap<RequestOtpDTO, CreateUserOtpDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OtpCode, opt => opt.Ignore()) // OtpCode sẽ được tạo bởi backend
                .ForMember(dest => dest.ExpirationTime, opt => opt.Ignore()); // ExpirationTime sẽ được tạo bởi backend

        
        }
    }
}
