using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Server.Workers;
using LettersAnalyzer.Shared.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace LettersAnalyzer.Server.DataAccess
{
    public class ArtWorkService : IArtWorkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFrequencyCounter _frequencyCounter;

        public ArtWorkService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _frequencyCounter = new FrequencyCounter(this);
        }

        public async Task AddArtWorkAsync(ArtWork artWork, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkRecord
                .InsertOneAsync(
                artWork,
                new InsertOneOptions()
                {
                    BypassDocumentValidation = true,
                },
                cancellationToken);
        }

        
        public async Task AddArtWorkBodyAsync(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default)
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

        public async Task DeleteArtWorkAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleteWork = _context.ArtWorkRecord
                .DeleteOneAsync(it => it.Id == id, cancellationToken);
            var deleteBody = _context.ArtWorkBodyRecord
                .DeleteOneAsync(it => it.ArtWorkId == id, cancellationToken);
            await deleteWork;
            await deleteBody;
        }

        public async Task<List<ArtWork>> GetAllArtWorksAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkRecord.FindAsync(_ => true, cancellationToken: cancellationToken))
                .ToList(cancellationToken: cancellationToken);
        }

        public async Task<List<ArtWorkBody>> GetArtWorkBodiesAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkBodyRecord.FindAsync(_ => true, cancellationToken: cancellationToken))
                .ToList(cancellationToken: cancellationToken);
        }

        public async Task<ArtWorkBody> GetWorkBodyDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkBodyRecord.FindAsync(body => body.Id == id, cancellationToken: cancellationToken))
                .FirstOrDefault(cancellationToken);
        }

        public async Task<ArtWork> GetWorkDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return (await _context.ArtWorkRecord.FindAsync(body => body.Id == id, cancellationToken: cancellationToken))
                .FirstOrDefault(cancellationToken);
        }

        public async Task UpdateArtWorkAsync(ArtWork artWork, CancellationToken cancellationToken = default)
        {
            await _context.ArtWorkRecord
                .ReplaceOneAsync(body => body.Id == artWork.Id, artWork, cancellationToken: cancellationToken);
        }

        public async Task UpdateArtWorkBodyAsync(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default)
        {
            
            await _context.ArtWorkBodyRecord
                .ReplaceOneAsync(body => body.Id == artWorkBody.Id, artWorkBody, cancellationToken: cancellationToken);
        }

        public async Task PostArtWorkWithBody(ArtWork artWork, CancellationToken cancellationToken = default)
        {
            await AddArtWorkAsync(artWork, cancellationToken);
            var artWorkBody = new ArtWorkBody()
            {
                ArtWorkId = artWork.Id,
                Body = artWork.Body,
            };
            await AddArtWorkBodyAsync(artWorkBody, cancellationToken);
            artWork.ArtWorkBodyId = artWorkBody.Id;
            await UpdateArtWorkAsync(artWork, cancellationToken);
            await _frequencyCounter.ProcessBodyAsync(artWorkBody, cancellationToken);
        }

        public async Task<FrequencyReport> GetFrequencyReportAsync(
            Expression<Func<ArtWork, string>> gropByexpression,
            string groupByName,
            CancellationToken cancellationToken = default)
        {
            var aggregate = await _context.ArtWorkRecord.Aggregate().ToListAsync();
            var frequencies =
                aggregate.AsQueryable().GroupBy(
                gropByexpression,
                (key, works) => new FrequencyLabel
                {
                    Label = key,
                    Value = works
                    .SelectMany(gr => gr.NormalizedLetterFrequency)
                    .GroupBy(it => it.Key)
                    .OrderBy(it => it.Key)
                    .Select(
                        it => new LetterCount()
                        {
                            Letter = it.Key,
                            Count = it.Average(it => it.Value)
                        })
                    .ToList()
                }).ToList();
            var result = new FrequencyReport()
            {
                Frequencies = frequencies,
                GroupBy = groupByName
            };
            return result;
        }
    }
}
