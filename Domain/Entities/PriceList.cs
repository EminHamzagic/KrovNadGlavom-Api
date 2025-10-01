namespace krov_nad_glavom_api.Domain.Entities
{
    public class PriceList
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public decimal GaragePrice { get; set; }
        public decimal PricePerM2 { get; set; }
        public decimal PenaltyPerM2 { get; set; }
    }
}