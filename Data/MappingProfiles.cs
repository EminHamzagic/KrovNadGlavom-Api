using AutoMapper;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using krov_nad_glavom_api.Data.DTO.Garage;
using krov_nad_glavom_api.Data.DTO.PriceList;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //User mappings
            CreateMap<UserToAddDto, User>().ReverseMap();
            CreateMap<UserToReturnDto, User>().ReverseMap();

            //ConstructionCompany mappings
            CreateMap<ConstructionCompanyToAddDto, ConstructionCompany>().ReverseMap();

            //Building mappings
            CreateMap<BuildingToAddDto, Building>().ReverseMap();
            CreateMap<Building, BuildingToUpdateDto>().ReverseMap();

            //Apartment mappings
            CreateMap<Apartment, ApartmentToAddDto>().ReverseMap();
            CreateMap<Apartment, ApartmentToUpdateDto>().ReverseMap();

            //PriceList mappings
            CreateMap<PriceList, PriceListToAddDto>().ReverseMap();

            //Garage mappings
            CreateMap<Garage, GarageToAddDto>().ReverseMap();
        }
    }
}