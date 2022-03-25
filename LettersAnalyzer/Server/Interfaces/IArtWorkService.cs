using LettersAnalyzer.Shared.Models;
using System.Linq.Expressions;

namespace LettersAnalyzer.Server.Interfaces
{
    public interface IArtWorkService
    {
        Task<List<ArtWork>> GetAllArtWorksAsync(CancellationToken cancellationToken = default);

        Task<ArtWork> GetWorkDetailsAsync(string id, CancellationToken cancellationToken = default);

        Task<List<ArtWorkBody>> GetArtWorkBodiesAsync(CancellationToken cancellationToken = default);

        Task<ArtWorkBody> GetWorkBodyDetailsAsync(string id, CancellationToken cancellationToken = default);

        Task AddArtWorkAsync(ArtWork artWork, CancellationToken cancellationToken = default);

        Task AddArtWorkBodyAsync(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default);

        Task DeleteArtWorkAsync(string id, CancellationToken cancellationToken = default);

        Task UpdateArtWorkAsync(ArtWork artWork, CancellationToken cancellationToken = default);

        Task UpdateArtWorkBodyAsync(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default);

        Task PostArtWorkWithBody(ArtWork artWork, CancellationToken cancellationToken = default);

        Task<FrequencyReport> GetFrequencyReportAsync(
            Expression<Func<ArtWork, string>> gropByexpression,
            string groupByName,
            CancellationToken cancellationToken = default);
    }
}
