using AutoMapper;
using krov_nad_glavom_api.Data.DTO.Agency;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;
using krov_nad_glavom_api.Data.DTO.Contract;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Data.DTO.Garage;
using krov_nad_glavom_api.Data.DTO.Installment;
using krov_nad_glavom_api.Data.DTO.PriceList;
using krov_nad_glavom_api.Data.DTO.Reservation;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Data.DTO.UserAgencyFollow;
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
            CreateMap<Building, BuildingToReturnDto>().ReverseMap();
            CreateMap<Building, BuildingEndDateToExtendDto>().ReverseMap();

            //Apartment mappings
            CreateMap<Apartment, ApartmentToAddDto>().ReverseMap();
            CreateMap<Apartment, ApartmentToUpdateDto>().ReverseMap();
            CreateMap<Apartment, ApartmentToReturnDto>().ReverseMap();

            //PriceList mappings
            CreateMap<PriceList, PriceListToAddDto>().ReverseMap();

            //Garage mappings
            CreateMap<Garage, GarageToAddDto>().ReverseMap();
            CreateMap<Garage, GarageToUpdateDto>().ReverseMap();

            //Agency mappings
            CreateMap<Agency, AgencyToAddDto>().ReverseMap();
            CreateMap<Agency, AgencyToReturnDto>().ReverseMap();

            //AgencyRequest mappings
            CreateMap<AgencyRequest, AgencyRequestToAddDto>().ReverseMap();
            CreateMap<AgencyRequest, AgencyRequestToUpdateDto>().ReverseMap();
            CreateMap<AgencyRequest, AgencyRequestToReturnDto>().ReverseMap();

            //UserAgencyFollow mappings
            CreateMap<UserAgencyFollow, UserAgencyFollowToAddDto>().ReverseMap();

            //DiscountRequest mappings
            CreateMap<DiscountRequest, DiscountRequestToAddDto>().ReverseMap();
            CreateMap<DiscountRequest, DiscountRequestToReturnDto>().ReverseMap();
            CreateMap<DiscountRequest, DiscountRequestToUpdateDto>().ReverseMap();

            //Contract mappings
            CreateMap<Contract, ContractToAddDto>().ReverseMap();
            CreateMap<Contract, ContractToReturnDto>().ReverseMap();

            //Installment mappings
            CreateMap<Installment, InstallmentToAddDto>().ReverseMap();

            //Reservation mappings
            CreateMap<Reservation, ReservationToAddDto>().ReverseMap();
        }
    }
}