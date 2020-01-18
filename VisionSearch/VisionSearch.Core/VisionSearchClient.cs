using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VisionSearch.Core.Models;

namespace VisionSearch.Core
{
    public class VisionSearchClient : IVisionSearchClient
    {
        private readonly IComputerVisionClient visionClient;
        private readonly IImageSearchClient searchClient;

        private readonly List<VisualFeatureTypes> visualFeatures;
        private readonly List<Details> visionDetails;

        public VisionSearchClient(IComputerVisionClient visionClient, IImageSearchClient searchClient)
        {
            this.visionClient = visionClient;
            this.searchClient = searchClient;

            // Create a list that defines the features to be extracted from the image. 
            visualFeatures = new List<VisualFeatureTypes>
            {
                VisualFeatureTypes.Description,
            };

            visionDetails = new List<Details>
            {
                 Details.Celebrities,
                 Details.Landmarks
            };
        }

        public async Task<SearchResult> SearchAsync(Stream image)
        {
            var imageAnalysisResult = await visionClient.AnalyzeImageInStreamAsync(image, visualFeatures, visionDetails);
            var description = ExtractDescription(imageAnalysisResult);

            var searchResult = await searchClient.Images.SearchAsync(description);

            var result = new SearchResult(description, searchResult.Value);
            return result;
        }

        private static string ExtractDescription(ImageAnalysis imageAnalysis)
        {
            return imageAnalysis.Categories?.FirstOrDefault(c => c.Name == "people_" || c.Name == "people_portrait")?.Detail?.Celebrities?.FirstOrDefault()?.Name
                ?? imageAnalysis.Categories?.FirstOrDefault()?.Detail?.Landmarks?.FirstOrDefault()?.Name
                ?? imageAnalysis.Description.Captions.FirstOrDefault().Text;
        }
    }
}
