namespace LettersAnalyzer.Shared.Models
{
    public class FrequencyReport
    {
        public string GroupBy { get; set; } = string.Empty;
        public List<FrequencyLabel> Frequencies { get; set; } = new List<FrequencyLabel>();
    }
}
