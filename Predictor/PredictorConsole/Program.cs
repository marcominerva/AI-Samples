using Microsoft.ML;
using SentimentAnalysis;
using SentimentAnalysis.DataModels;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PredictorConsole
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var mlContext = new MLContext();
            var mlModel = await GetLocalModelAsync(mlContext);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(mlModel);

            var sentimentAnalyzer = new SentimentAnalyzer(predictionEngine);

            string text;
            do
            {
                Console.WriteLine("Write your sentence (empty line to exit): ");

                text = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var result = await sentimentAnalyzer.AnalyzeAsync(text);
                    Console.WriteLine($"Sentiment: {result.Sentiment} (Confidence: {result.Confidence:P2})");
                    Console.WriteLine();
                }

            } while (!string.IsNullOrWhiteSpace(text));
        }

        private static Task<ITransformer> GetLocalModelAsync(MLContext mlContext)
        {
            var modelPath = Path.Combine(AppContext.BaseDirectory, "MLModels", "sentiment_model.zip");

            var model = mlContext.Model.Load(modelPath, out _);
            return Task.FromResult(model);
        }

        private static async Task<ITransformer> GetRemoteModelAsync(MLContext mlContext)
        {
            using var httpClient = new HttpClient();
            using var modelStream = await httpClient.GetStreamAsync("https://github.com/dotnet/samples/raw/master/machine-learning/models/sentimentanalysis/sentiment_model.zip");

            var model = mlContext.Model.Load(modelStream, out _);
            return model;
        }
    }
}
