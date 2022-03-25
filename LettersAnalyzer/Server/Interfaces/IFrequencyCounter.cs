using LettersAnalyzer.Shared.Models;

namespace LettersAnalyzer.Server.Interfaces
{
    public interface IFrequencyCounter
    {
        Task ProcessBodyAsync(ArtWorkBody body, CancellationToken cancellationToken = default);
    }
}
