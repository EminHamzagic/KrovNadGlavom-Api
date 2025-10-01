namespace krov_nad_glavom_api.Data.DTO.Garage
{
    public class GarageToUpdateDto
    {
        public string SpotNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string ApartmentId { get; set; }
    }
}