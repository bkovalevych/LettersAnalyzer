using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LettersAnalyzer.Shared.Models
{
    public class LetterFrequesncy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Dictionary<string, int> Frequence { get; set; }
    }
}
