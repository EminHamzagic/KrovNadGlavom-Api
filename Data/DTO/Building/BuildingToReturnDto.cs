using krov_nad_glavom_api.Data.DTO.ConstructionCompany;

namespace krov_nad_glavom_api.Data.DTO.Building
{
    public class BuildingToReturnDto
    {
        public string Id { get; set; }
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
        public List<Domain.Entities.Apartment> Apartments { get; set; }
        public List<Domain.Entities.Garage> Garages { get; set; }
        public Domain.Entities.PriceList PriceList { get; set; }
        public Domain.Entities.ConstructionCompany Company { get; set; }
    }
}