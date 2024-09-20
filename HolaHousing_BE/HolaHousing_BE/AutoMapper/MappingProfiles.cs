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
        }

    }
}
