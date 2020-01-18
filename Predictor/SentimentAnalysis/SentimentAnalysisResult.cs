namespace SentimentAnalysis
{
    public class SentimentAnalysisResult
    {
        public string Text { get; set; }

        public string Sentiment { get; set; }

        public float Confidence { get; set; }

        public bool IsPositive { get; set; }
    }
}
