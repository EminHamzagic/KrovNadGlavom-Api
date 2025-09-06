namespace krov_nad_glavom_api.Domain.Entities
{
    public class Building
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
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}