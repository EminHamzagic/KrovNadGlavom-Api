using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Garage;
using krov_nad_glavom_api.Data.DTO.PriceList;

namespace krov_nad_glavom_api.Data.DTO.Building
{
    public class BuildingToAddDto
    {
        public string CompanyId { get; set; }
        public string ParcelNumber { get; set; }
        public decimal Area { get; set; }
        public int FloorCount { get; set; }
        public int ElevatorCount { get; set; }
        public int GarageSpotCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ExtendedUntil { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
        public List<ApartmentToAddDto> Apartments { get; set; }
        public List<GarageToAddDto> Garages { get; set; }
        public PriceListToAddDto PriceList { get; set; }
    }
}