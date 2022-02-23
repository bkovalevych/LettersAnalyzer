using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LettersAnalyzer.Shared.Models
{
    public class ArtWorkBody
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ArtWorkId { get; set; }
        public string Body { get; set; }
    }
}
