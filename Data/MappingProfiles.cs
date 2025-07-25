using AutoMapper;
using krov_nad_glavom_api.Controllers;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserToAddDto, User>().ReverseMap();
            CreateMap<UserToReturnDto, User>().ReverseMap();
        }
    }
}