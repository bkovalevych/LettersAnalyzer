using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LettersAnalyzer.Shared.Models
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        public ApplicationDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _mongoDatabase = client.GetDatabase("ArtWorksDB");
        }
        public IMongoCollection<ArtWork> ArtWorkRecord
        {
            get
            {
                return _mongoDatabase.GetCollection<ArtWork>("ArtWorkRecord");
            }
        }
        public IMongoCollection<ArtWorkBody> ArtWorkBodyRecord
        {
            get
            {
                return _mongoDatabase.GetCollection<ArtWorkBody>("ArtWorkBodyRecord");
            }
        }
    }
}
