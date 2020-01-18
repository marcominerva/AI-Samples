using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System.Collections.Generic;

namespace VisionSearch.Core.Models
{
    public class SearchResult
    {
        public string Description { get; }

        public IEnumerable<ImageObject> Images { get; }

        public SearchResult(string description, IEnumerable<ImageObject> images)
        {
            Description = description;
            Images = images;
        }
    }
}
