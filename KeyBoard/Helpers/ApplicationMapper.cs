using KeyBoard.Data;
using KeyBoard.DTOs;
using AutoMapper;
namespace KeyBoard.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
