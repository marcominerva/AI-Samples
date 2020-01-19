using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Predictor;
using Predictor.DataModels;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Predictor
{
    public class Startup : FunctionsStartup
    {
        private readonly string environment;
        private readonly string modelPath;

        public Startup()
        {
            environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            if (environment == "Development")
            {
                modelPath = Path.Combine("MLModels", "sentiment_model.zip");
            }
            else
            {
                var deploymentPath = @"D:\home\site\wwwroot\";
                modelPath = Path.Combine(deploymentPath, "MLModels", "sentiment_model.zip");
            }
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddPredictionEnginePool<SentimentData, SentimentPrediction>()
                .FromFile(modelName: "SentimentAnalysisModel", filePath: modelPath, watchForChanges: true);

            //builder.Services.AddPredictionEnginePool<SentimentData, SentimentPrediction>()
            //  .FromUri(
            //      modelName: "SentimentAnalysisModel",
            //      uri: "https://github.com/dotnet/samples/raw/master/machine-learning/models/sentimentanalysis/sentiment_model.zip",
            //      period: TimeSpan.FromMinutes(10));
        }
    }
}
