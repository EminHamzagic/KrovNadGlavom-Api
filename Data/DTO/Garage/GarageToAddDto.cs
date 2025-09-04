namespace krov_nad_glavom_api.Data.DTO.Garage
{
    public class GarageToAddDto
    {
        public string BuildingId { get; set; }
        public string SpotNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}