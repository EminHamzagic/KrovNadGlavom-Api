namespace krov_nad_glavom_api.Data.DTO.Building
{
    public class BuildingToUpdateDto
    {
        public string ParcelNumber { get; set; }
        public decimal Area { get; set; }
        public int FloorCount { get; set; }
        public int ElevatorCount { get; set; }
        public int GarageSpotCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ExtendedUntil { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Description { get; set; }
    }
}