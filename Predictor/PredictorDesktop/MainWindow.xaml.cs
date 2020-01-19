using Predictor.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace PredictorDesktop
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient;
        private const string serviceUrl = "http://localhost:7071/api/";

        public MainWindow()
        {
            InitializeComponent();

            httpClient = new HttpClient { BaseAddress = new Uri(serviceUrl) };
        }

        private async void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            var jsonContent = JsonSerializer.Serialize(new Request { Text = txtSentence.Text });
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("AnalyzeSentiment", requestContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var prediction = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            lblAnalyzeResult.Text = $"Sentiment: {prediction.Sentiment} (Positive Probability: {prediction.PositiveProbability:P2})";
        }
    }
}
