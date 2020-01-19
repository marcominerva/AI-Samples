using Microsoft.ML.Data;

namespace PredictorConsole.DataModels
{
    public class SentimentPrediction
    {

        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
