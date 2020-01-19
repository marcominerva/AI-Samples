using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using Newtonsoft.Json;
using Predictor.DataModels;
using Predictor.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Predictor
{
    public class SentimentAnalyzer
    {
        private readonly PredictionEnginePool<SentimentData, SentimentPrediction> predictionEnginePool;

        public SentimentAnalyzer(PredictionEnginePool<SentimentData, SentimentPrediction> predictionEnginePool)
        {
            this.predictionEnginePool = predictionEnginePool;
        }

        [FunctionName("AnalyzeSentiment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            //Parse HTTP Request Body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Request>(requestBody);

            //Make Prediction
            var prediction = predictionEnginePool.Predict(modelName: "SentimentAnalysisModel", new SentimentData { SentimentText = data.Text });

            var result = new Response
            {
                Text = data.Text,
                IsPositive = Convert.ToBoolean(prediction.Prediction),
                Sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative",
                PositiveProbability = prediction.Probability
            };

            //Return Prediction
            return new OkObjectResult(result);
        }
    }
}
