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

        public VisionSearchClient(IComputerVisionClient visionClient, IImageSearchClient searchClient)
        {
            this.visionClient = visionClient;
            this.searchClient = searchClient;

            // Creating a list that defines the features to be extracted from the image. 
            visualFeatures = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Description,
            };
        }

        public async Task<SearchResult> SearchAsync(Stream image)
        {
            var imageAnalysisResult = await visionClient.AnalyzeImageInStreamAsync(image, visualFeatures);
            var description = imageAnalysisResult.Description.Captions.FirstOrDefault().Text;

            var searchResult = await searchClient.Images.SearchAsync(description);

            var result = new SearchResult(description, searchResult.Value);
            return result;
        }
    }
}
