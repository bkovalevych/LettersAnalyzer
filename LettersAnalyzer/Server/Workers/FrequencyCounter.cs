using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Shared.Models;

namespace LettersAnalyzer.Server.Workers
{
    public class FrequencyCounter : IFrequencyCounter
    {
        private readonly IArtWorkService _artWorkService;

        public FrequencyCounter(IArtWorkService artWorkService)
        {
            _artWorkService = artWorkService;
        }

        public async Task ProcessBodyAsync(ArtWorkBody body, CancellationToken cancellationToken = default)
        {
            if (body == null || body.Body == null || body.ArtWorkId == null)
            {
                return;
            }
            var result = CountFrequency(body.Body);
            var artWork = await _artWorkService.GetWorkDetailsAsync(body.ArtWorkId, cancellationToken);
            artWork.LetterFrequency = result;
            artWork.NormalizedLetterFrequency = CountNormalizedFrequency(result, body.Body.Length);
            await _artWorkService.UpdateArtWorkAsync(artWork, cancellationToken);
        }

        private static Dictionary<string, int> CountFrequency(string input)
        {
            var result = input.Where(
                symbol => 'a' <= char.ToLower(symbol) 
                && char.ToLower(symbol) <= 'z')
                .GroupBy(symbol => char.ToString(symbol).ToLower())
                .ToDictionary(
                grouping => grouping.Key, 
                grouping => grouping.Sum(_ => 1));
            return result;
        }

        private static Dictionary<string, double> CountNormalizedFrequency(
            Dictionary<string, int> frequency,
            int length
            )
        {
            return frequency.ToDictionary(
                grouping => grouping.Key,
                grouping => (double)grouping.Value / length);
        }
    }
}
