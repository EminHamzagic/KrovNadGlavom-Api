namespace krov_nad_glavom_api.Data.DTO.Apartment
{
    public class ApartmentWithBuildingDto
    {
        public Domain.Entities.Apartment Apartment { get; set; }
        public Domain.Entities.Building Building { get; set; }
    }
}