namespace Predictor.Models
{
    public class Response
    {
        public string Text { get; set; }

        public string Sentiment { get; set; }

        public bool IsPositive { get; set; }

        public float PositiveProbability { get; set; }
    }
}
