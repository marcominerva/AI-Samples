using System.IO;
using System.Threading.Tasks;
using VisionSearch.Core.Models;

namespace VisionSearch.Core;

public interface IVisionSearchClient
{
    Task<SearchResult> SearchAsync(Stream image);
}
