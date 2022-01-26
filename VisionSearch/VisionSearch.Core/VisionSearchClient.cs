using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using VisionSearch.Core.Models;

namespace VisionSearch.Core;

public class VisionSearchClient : IVisionSearchClient
{
    private readonly IComputerVisionClient visionClient;
    private readonly IImageSearchClient searchClient;

    private readonly IList<VisualFeatureTypes?> visualFeatures;
    private readonly IList<Details?> visionDetails;

    public VisionSearchClient(IComputerVisionClient visionClient, IImageSearchClient searchClient)
    {
        this.visionClient = visionClient;
        this.searchClient = searchClient;

        // Create a list that defines the features to be extracted from the image. 
        visualFeatures = new List<VisualFeatureTypes?>
        {
            VisualFeatureTypes.Description,
        };

        visionDetails = new List<Details?>
        {
            Details.Celebrities,
            Details.Landmarks
        };
    }

    public async Task<SearchResult> SearchAsync(Stream image)
    {
        var imageAnalysisResult = await visionClient.AnalyzeImageInStreamAsync(image, visualFeatures, visionDetails);
        var description = ExtractDescription(imageAnalysisResult);

        if (!string.IsNullOrWhiteSpace(description))
        {
            var searchResult = await searchClient.Images.SearchAsync(description);

            var result = new SearchResult(description, searchResult.Value);
            return result;
        }

        return null;

        static string ExtractDescription(ImageAnalysis imageAnalysis)
            => imageAnalysis.Categories?.FirstOrDefault(c => c.Name == "people_" || c.Name == "people_portrait")?.Detail?.Celebrities?.FirstOrDefault()?.Name
                ?? imageAnalysis.Categories?.FirstOrDefault()?.Detail?.Landmarks?.FirstOrDefault()?.Name
                ?? imageAnalysis.Description.Captions?.FirstOrDefault()?.Text;
    }
}
