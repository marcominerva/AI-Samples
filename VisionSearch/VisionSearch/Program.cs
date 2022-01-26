using VisionSearch;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    builder.AddJsonFile("appsettings.local.json", optional: true);
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

app.Run();