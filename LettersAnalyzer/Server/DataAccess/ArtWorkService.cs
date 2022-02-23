using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Shared.Models;
using MongoDB.Driver;

namespace LettersAnalyzer.Server.DataAccess
{
    public class ArtWorkService : IArtWorkService
    {
        private readonly ApplicationDbContext _context;

        public ArtWorkService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddArtWork(ArtWork artWork, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkRecord
                .InsertOneAsync(
                artWork,
                new InsertOneOptions()
                {
                    BypassDocumentValidation = true,
                },
                cancellationToken);
            var a = _context.ArtWorkRecord
                .Aggregate()
                .Group(
                it => it.Century,
                grouping => new
                {
                    Century = grouping.Key,
                    Frequency = grouping
                    .SelectMany(gr => gr.LetterFrequesncy.Frequence)
                    .GroupBy(it => it)
                    .ToDictionary(it => it.Key, it => it.Average(it => it.Value))
                });
        }

        
        public async Task AddArtWorkBody(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkBodyRecord
                .InsertOneAsync(
                artWorkBody,
                new InsertOneOptions()
                {
                    BypassDocumentValidation = true,
                },
                cancellationToken);
        }

        public async Task DeleteArtWork(string id, CancellationToken cancellationToken = default)
        {
            var deleteWork = _context.ArtWorkRecord
                .DeleteOneAsync(it => it.Id == id, cancellationToken);
            var deleteBody = _context.ArtWorkBodyRecord
                .DeleteOneAsync(it => it.ArtWorkId == id, cancellationToken);
            await deleteWork;
            await deleteBody;
        }

        public async Task<List<ArtWork>> GetAllArtWorks(CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkRecord.FindAsync(_ => true, cancellationToken: cancellationToken))
                .ToList(cancellationToken: cancellationToken);
        }

        public async Task<List<ArtWorkBody>> GetArtWorkBodies(CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkBodyRecord.FindAsync(_ => true, cancellationToken: cancellationToken))
                .ToList(cancellationToken: cancellationToken);
        }

        public async Task<ArtWorkBody> GetWorkBodyDetails(string id, CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkBodyRecord.FindAsync(body => body.Id == id, cancellationToken: cancellationToken))
                .FirstOrDefault(cancellationToken);
        }

        public async Task<ArtWork> GetWorkDetails(string id, CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkRecord.FindAsync(body => body.Id == id, cancellationToken: cancellationToken))
                .FirstOrDefault(cancellationToken);
        }

        public async Task UpdateArtWork(ArtWork artWork, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkRecord
                .ReplaceOneAsync(body => body.Id == artWork.Id, artWork, cancellationToken: cancellationToken);
        }

        public async Task UpdateArtWorkBody(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkBodyRecord
                .ReplaceOneAsync(body => body.Id == artWorkBody.Id, artWorkBody, cancellationToken: cancellationToken);
        }
    }
}
