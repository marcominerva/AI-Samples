using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using VisionSearch.Core;
using VisionSearch.Models;

namespace VisionSearch;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();

        var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddScoped<IComputerVisionClient, ComputerVisionClient>(sp =>
        {
            var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(appSettings.VisionSubscriptionKey))
            {
                Endpoint = appSettings.VisionEndpoint
            };

            return client;
        });

        services.AddScoped<IImageSearchClient, ImageSearchClient>(sp =>
        {
            var client = new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(appSettings.BingSubscriptionKey))
            {
                Endpoint = appSettings.VisionEndpoint
            };

            return client;
        });

        services.AddScoped<IVisionSearchClient, VisionSearchClient>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}
