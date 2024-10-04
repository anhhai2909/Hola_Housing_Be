using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Models;

namespace NguyenAnhHai_Assignment1_PRN231.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Amentity, AmentityDTO>();
            CreateMap<AmentityDTO, Amentity>();
            CreateMap<Property, PropertyDTO>()
            .ForMember(dest => dest.PropertyImages, opt => opt.MapFrom(src => src.PropertyImages))
            .ForMember(dest => dest.Amentities, opt => opt.MapFrom(src => src.Amentities));
            CreateMap<PropertyDTO, Property>();
            CreateMap<PropertyImage, PropertyImageDTO>();
            CreateMap<PostPrice, PostPriceDTO>();
            CreateMap<PostType, PostTypeDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<New, NewDTO>()
            .ForMember(dest => dest.CreatedByNavigation, opt => opt.MapFrom(src => src.CreatedByNavigation))
            .ForMember(dest => dest.PartContents, opt => opt.MapFrom(src => src.PartContents))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));
            CreateMap<Tag, TagDTO>();
            CreateMap<RelatedNew, RelatedNewDTO>()
                .ForMember(dest => dest.New, opt => opt.MapFrom(src => src.New));
            CreateMap<PartContent, PartContentDTO>();
            CreateMap<PostType, PostTypeDTO>();
        }
    }
}
