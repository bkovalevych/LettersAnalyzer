using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Shared.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace LettersAnalyzer.Server.Workers
{
    public class SeedDataHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IArtWorkService _artWorkService;
        public SeedDataHelper(IArtWorkService artWorkService)
        {
            _httpClient = new HttpClient();
            _artWorkService = artWorkService;
        }

        public async Task LoadInitialData()
        {
            string pattern = "https://www.gutenberg.org/files/{0}/{0}-0.txt";
            List<int> anchors = new List<int>() { 3175, 56166, 4679 };
            var links = new List<string>();
            foreach (int anchor in anchors)
            {
                for(int startAnchor = anchor; startAnchor < anchor + 5; startAnchor++)
                {
                    links.Add(string.Format(pattern, startAnchor));
                }
            }
            foreach (var link in links)
            {
                using var stream = await _httpClient.GetStreamAsync(link);
                using var t = new StreamReader(stream);
                var artWork = ProcessOneBookFromGootenberg(t);
                await _artWorkService.PostArtWorkWithBody(artWork);
            }
        }

        private static ArtWork ProcessOneBookFromGootenberg(StreamReader stream)
        {
            var artWork = new ArtWork();
            PrepareMetaData(stream, artWork);
            PrepareContent(stream, artWork);
            return artWork;
        }

        private static void PrepareMetaData(StreamReader stream, ArtWork artWork)
        {
            string? line;
            int countFounded = 0;
            while ((line = stream.ReadLine()) != null && countFounded < 3)
            {
                if (line.StartsWith("Title:"))
                {
                    artWork.Name = line[(line.IndexOf(' ') + 1)..];
                    ++countFounded;
                }
                else if ((line.StartsWith("Editor:") || line.StartsWith("Author:")) && artWork.Author == null)
                {
                    artWork.Author = line[(line.IndexOf(' ') + 1)..];
                    ++countFounded;
                }
                else if (line.StartsWith("Release Date:"))
                {
                    int startIndex = 14;
                    var dateString = line[startIndex..line.IndexOf('[')];
                    DateOnly.TryParse(dateString, out var date);
                    artWork.Century = date.Year / 100 + 1;
                    ++countFounded;
                }
            }
        }

        private static void PrepareContent(StreamReader stream, ArtWork artWork)
        {
            var content = new StringBuilder();
            string? line;
            var startPattern = new Regex(@"\s*\*\*\*\s*START", RegexOptions.IgnoreCase);
            var endPattern = new Regex(@"\s*\*\*\*\s*END", RegexOptions.IgnoreCase);
            bool isStarted = false;
            while ((line = stream.ReadLine()) != null)
            {
                if (startPattern.IsMatch(line))
                {
                    isStarted = true;
                    continue;
                }
                else if (endPattern.IsMatch(line))
                {
                    break;
                }
                else if (isStarted)
                {
                    content.AppendLine(line);
                }
            }
            artWork.Body = content.ToString();
        }
    }
}
