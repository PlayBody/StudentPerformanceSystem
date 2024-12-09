using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Data;
using StudentPerformanceSystem.ETL;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ThirdPartyApiOptions>(builder.Configuration.GetSection("ThirdPartyApi"));
builder.Services.AddHttpClient();
builder.Services.AddDbContext<StudentPerformanceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddHostedService<Worker>();
try
{
    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application startup failed: {ex.Message}");
}
