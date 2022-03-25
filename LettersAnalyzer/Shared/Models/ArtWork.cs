using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LettersAnalyzer.Shared.Models
{
    public class ArtWork
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Author { get; set; }
        
        public string? Name { get; set; }

        public string? Country { get; set; }

        //public DateOnly Created { get; set; }

        public int Century { get; set; }

        public string? ArtWorkBodyId { get; set; }

        public Dictionary<string, int> LetterFrequency { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, double> NormalizedLetterFrequency { get; set; } = new Dictionary<string, double>();

        [BsonIgnore]
        public string? Body { get; set; }
    }
}
