using krov_nad_glavom_api.Data.DTO.Building;

namespace krov_nad_glavom_api.Data.DTO.Apartment
{
    public class ApartmentToReturnDto
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
        public bool CanRequestDiscount { get; set; }
        public Domain.Entities.Reservation Reservation { get; set; }
        public BuildingToReturnDto Building { get; set; }
        public Domain.Entities.Agency Agency { get; set; }
        public Domain.Entities.DiscountRequest DiscountRequest { get; set; }
        public List<Domain.Entities.Garage> Garages { get; set; }
    }
}