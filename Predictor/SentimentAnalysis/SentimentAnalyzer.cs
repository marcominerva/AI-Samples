using Microsoft.ML;
using SentimentAnalysis.DataModels;
using System;
using System.Threading.Tasks;

namespace SentimentAnalysis
{
    public class SentimentAnalyzer : ISentimentAnalyzer
    {
        private readonly PredictionEngine<SentimentData, SentimentPrediction> predictionEngine;

        public SentimentAnalyzer(PredictionEngine<SentimentData, SentimentPrediction> predictionEngine)
        {
            this.predictionEngine = predictionEngine;
        }

        public Task<SentimentAnalysisResult> AnalyzeAsync(string text)
        {
            //Make Prediction
            var prediction = predictionEngine.Predict(new SentimentData { SentimentText = text });

            //Analyze prediction
            var sentiment = new SentimentAnalysisResult
            {
                Text = text,
                Confidence = prediction.Probability,
                IsPositive = Convert.ToBoolean(prediction.Prediction),
                Sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative"
            };

            return Task.FromResult(sentiment);
        }
    }
}
