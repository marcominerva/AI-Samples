using System.Threading.Tasks;

namespace SentimentAnalysis
{
    public interface ISentimentAnalyzer
    {
        Task<SentimentAnalysisResult> AnalyzeAsync(string text);
    }
}