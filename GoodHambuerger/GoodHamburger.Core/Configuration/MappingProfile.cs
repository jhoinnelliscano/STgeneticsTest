using AutoMapper;
using GoodHamburger.Core.DTOs;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Core.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderRequestDto, OrderEntity>()
                .ForMember(dest => dest.Details, opt => opt.Ignore())
                .ForMember(dest => dest.Sandwich, opt => opt.Ignore());

            CreateMap<OrderEntity, OrderResponseDto>()
                .ForMember(dest => dest.SandwichName, opt => opt.MapFrom(src => src.Sandwich.Name))
                .ForMember(dest => dest.Extras, opt => opt.MapFrom(src => src.Details.Select(d => d.Extra.Name)));
        }
    }
}
