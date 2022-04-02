namespace LettersAnalyzer.Shared.Models
{
    public class FrequencyLabel
    {
        public string Label { get; set; } = string.Empty;
        public List<LetterCount> Value { get; set; } = new List<LetterCount>();
    }
}
