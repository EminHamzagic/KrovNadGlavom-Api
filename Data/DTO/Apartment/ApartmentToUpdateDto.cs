namespace krov_nad_glavom_api.Data.DTO.Apartment
{
    public class ApartmentToUpdateDto
    {
        public string ApartmentNumber { get; set; }
        public decimal Area { get; set; }
        public int RoomCount { get; set; }
        public int BalconyCount { get; set; }
        public string Orientation { get; set; }
        public int Floor { get; set; }
        public bool IsAvailable { get; set; }
    }
}