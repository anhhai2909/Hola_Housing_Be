using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Models;

namespace NguyenAnhHai_Assignment1_PRN231.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Property, PropertyDTO>();
            CreateMap<PropertyImage, PropertyImageDTO>();
            CreateMap<PostPrice, PostPriceDTO>();
            CreateMap<PostType, PostTypeDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<New, NewDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<RelatedNew, RelatedNewDTO>();
            CreateMap<PartContent, PartContentDTO>();
            CreateMap<PostType, PostTypeDTO>();
        }
    }
}
