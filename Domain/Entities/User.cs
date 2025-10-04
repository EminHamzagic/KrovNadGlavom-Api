using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace krov_nad_glavom_api.Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public string ConstructionCompanyId { get; set; }
        public string AgencyId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAllowed { get; set; }
    }
}