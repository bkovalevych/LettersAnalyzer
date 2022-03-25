using LettersAnalyzer.Server.Settings;
using LettersAnalyzer.Shared.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LettersAnalyzer.Server.DataAccess
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        public ApplicationDbContext(IOptions<ArtWorkSettings> artWorkSettings)
        {
            var mongoClient = new MongoClient(
            artWorkSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(
                artWorkSettings.Value.DbName);
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
