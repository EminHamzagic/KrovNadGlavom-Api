namespace krov_nad_glavom_api.Domain.Entities
{
    public class Garage
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string SpotNumber { get; set; }
        public bool IsAvailable { get; set; }
    }
}