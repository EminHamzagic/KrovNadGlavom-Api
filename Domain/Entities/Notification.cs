namespace krov_nad_glavom_api.Domain.Entities
{
    public class Notification
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}