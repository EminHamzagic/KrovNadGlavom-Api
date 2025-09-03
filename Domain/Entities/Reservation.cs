namespace krov_nad_glavom_api.Domain.Entities
{
    public class Reservation
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ApartmentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}