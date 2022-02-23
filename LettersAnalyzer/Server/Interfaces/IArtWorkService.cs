using LettersAnalyzer.Shared.Models;

namespace LettersAnalyzer.Server.Interfaces
{
    public interface IArtWorkService
    {
        Task<List<ArtWork>> GetAllArtWorks(CancellationToken cancellationToken = default);

        Task<ArtWork> GetWorkDetails(string id, CancellationToken cancellationToken = default);

        Task<List<ArtWorkBody>> GetArtWorkBodies(CancellationToken cancellationToken = default);

        Task<ArtWorkBody> GetWorkBodyDetails(string id, CancellationToken cancellationToken = default);

        Task AddArtWork(ArtWork artWork, CancellationToken cancellationToken = default);

        Task AddArtWorkBody(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default);

        Task DeleteArtWork(string id, CancellationToken cancellationToken = default);

        Task UpdateArtWork(ArtWork artWork, CancellationToken cancellationToken = default);

        Task UpdateArtWorkBody(ArtWorkBody artWorkBody, CancellationToken cancellationToken = default);
    }
}
