using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using VisionSearch.Core;
using VisionSearch.Core.Models;

namespace VisionSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly IVisionSearchClient visionSearchClient;

        [BindProperty]
        public IFormFile Upload { get; set; }

        public SearchResult SearchResult { get; set; }

        public string ImageUrl { get; set; }

        public IndexModel(IWebHostEnvironment environment, IVisionSearchClient visionSearchClient)
        {
            this.environment = environment;
            this.visionSearchClient = visionSearchClient;
        }

        public async Task OnPostAsync()
        {
            if (Upload != null)
            {
                var fileName = Path.GetFileName(Upload.FileName);
                var imagePath = Path.Combine(environment.WebRootPath, "uploads", fileName);
                ImageUrl = Url.Content($"~/uploads/{fileName}");

                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await Upload.CopyToAsync(fileStream);


                using var stream = Upload.OpenReadStream();
                SearchResult = await visionSearchClient.SearchAsync(stream);
            }
            else
            {
                SearchResult = null;
            }
        }
    }
}
