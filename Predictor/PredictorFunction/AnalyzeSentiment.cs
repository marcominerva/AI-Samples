using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Predictor.Models;
using SentimentAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Predictor
{
    public class AnalyzeSentiment
    {
        private readonly ISentimentAnalyzer sentimentAnalyzer;

        public AnalyzeSentiment(ISentimentAnalyzer sentimentAnalyzer)
        {
            this.sentimentAnalyzer = sentimentAnalyzer;
        }

        [FunctionName("Analyze")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            //Parse HTTP Request Body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Request>(requestBody);

            //Make Prediction
            var prediction = await sentimentAnalyzer.AnalyzeAsync(data.Text);

            //Return Prediction
            return new OkObjectResult(prediction);
        }
    }
}
