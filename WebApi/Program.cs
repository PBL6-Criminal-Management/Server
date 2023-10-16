using Application.Extensions;
using Infrastructure.Extensions;
using Serilog;
using Shared.Extensions;
using System.Net;
using System.Net.Sockets;
using WebApi.Extensions;

Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");

IPHostEntry host;
string localIP = "?";
host = Dns.GetHostEntry(Dns.GetHostName());

foreach (IPAddress ip in host.AddressList)
{
    if (ip.AddressFamily == AddressFamily.InterNetwork)
    {
        localIP = ip.ToString();
        if (localIP == "127.0.0.1" || localIP == "::1")
        {
            builder.WebHost.UseUrls($"https://{localIP}:1234", $"http://{localIP}:5678");
        }
        break;
    }
}

// Add services to the container.
try
{
    builder.Host.AddAppConfigurations();

    builder.Services.AddApplicationExtensions();

    builder.Services.AddPersistenceInfrastructure(builder.Configuration);

    builder.Services.AddSharedExtensions(builder.Configuration);

    builder.Services.AddSwaggerExtension();

    //builder.Services.AddHangFire(builder.Configuration);

    builder.Services.AddApiversioningExtension();

    builder.Services.AddCorsExtensions();

    builder.Services.AddIdentityServices();

    builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));

    builder.Services.AddCurrentUserService();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CustomValidationFilter>(int.MinValue);
    });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddLazyCache();

    var app = builder.Build();

    app.UseRouting();

    app.UseFolderAsStatic();

    app.UseCors("CorsPolicy");

    app.UseSwaggerExtension();

    //app.UseHangfireExtension();

    app.UseErrorHandlingMiddleware();

    //app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    //app.UseHealthChecks("/health");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    // Seeding data
    app.InitializeDb();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}