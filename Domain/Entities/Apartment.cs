namespace krov_nad_glavom_api.Domain.Entities
{
    public class Apartment
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string ApartmentNumber { get; set; }
        public decimal Area { get; set; }
        public int RoomCount { get; set; }
        public int BalconyCount { get; set; }
        public string Orientation { get; set; }
        public int Floor { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}