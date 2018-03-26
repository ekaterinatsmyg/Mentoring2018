using AsyncTasks.Task3.DBModels;
using AsyncTasks.Task3.Mappers;
using AsyncTasks.Task3.Models;

namespace AsyncTasks.Task3.Registrars
{
    public static class MappingRegistrar
    {
        public static void Register()
        {
            Mapper.CreateMap<ProductModel, ProductViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.IsInBasket, opt => opt.MapFrom(src => src.IsInBasket));

            Mapper.CreateMap<ProductViewModel, ProductModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.IsInBasket, opt => opt.MapFrom(src => src.IsInBasket));
        }
    }
}