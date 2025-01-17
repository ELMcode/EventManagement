using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventManagement.Models
{
    public class EventStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int EventId { get; set; }
        public int TotalRegistrations { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
